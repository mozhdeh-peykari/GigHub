using GigHub.Models;
using GigHub.Persistence;
using GigHub.Repositories;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;
        public GigsController()
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
        }
        [Authorize]
        public ActionResult Create()
        {
            var vm = new GigFormViewModel
            {
                Heading = "Add a Gig",
                Genres = _unitOfWork.Genres.GetGenres()
            };
            return View("GigForm", vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Genres = _unitOfWork.Genres.GetGenres();
                return View("GigForm", vm);
            }
            var model = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = vm.GetDateTime(),
                Venue = vm.Venue,
                GenreId = vm.Genre
            };

            _unitOfWork.Gigs.AddGig(model);

            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }


        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _unitOfWork.Gigs.GetGig(id);

            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != userId)
                return new HttpUnauthorizedResult();

            var vm = new GigFormViewModel
            {
                Heading = "Edit a Gig",
                Genres = _unitOfWork.Genres.GetGenres(),
                Id = gig.Id,
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Venue = gig.Venue,
                Genre = gig.GenreId
            };
            return View("GigForm", vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Genres = _unitOfWork.Genres.GetGenres();
                return View("GigForm", vm);
            }

            var userId = User.Identity.GetUserId();

            var gig = _unitOfWork.Gigs.GetGigWithAttendees(vm.Id);

            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != userId)
                return new HttpUnauthorizedResult();

            gig.Modify(vm.Venue, vm.GetDateTime(), vm.Genre);

            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            var gigViewModel = new GigViewModel
            {
                Gigs = _unitOfWork.Gigs.GetGigsUserAttending(userId),
                ShowAction = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Going",
                Attendances = _unitOfWork.Attendances.GetFutureAttendances(userId).ToLookup(a => a.GigId)
            };

            return View("Gigs", gigViewModel);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _unitOfWork.Gigs.GetUpcomingGigsByArtist(userId);
            return View(gigs);
        }

        [Authorize]
        public ActionResult Search(GigViewModel gigViewModel)
        {
            return RedirectToAction("Index", "Home", new { query = gigViewModel.SearchTerm });
        }

        public ActionResult Details(int id)
        {
            var userId = User.Identity.GetUserId();

            var gig = _unitOfWork.Gigs.GetGig(id);

            if (gig == null)
                return HttpNotFound();

            var gigDetailViewModel = new GigDetailViewModel
            {
                Gig = gig
            };

            if (User.Identity.IsAuthenticated)
            {
                gigDetailViewModel.IsAttending = _unitOfWork.Attendances.GetAttendance(gig.Id, userId) != null;
                gigDetailViewModel.IsFollowing = _unitOfWork.Followings.GetFollowing(gig.ArtistId, userId) != null;
            }

            return View("Details", gigDetailViewModel);
        }
    }
}