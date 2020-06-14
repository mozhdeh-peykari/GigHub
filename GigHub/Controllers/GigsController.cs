using GigHub.Models;
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
        public GigsController()
        {
            _context = new ApplicationDbContext();
        }
        [Authorize]
        public ActionResult Create()
        {
            var vm = new GigFormViewModel
            {
                Heading = "Add a Gig",
                Genres = _context.Genres.ToList()
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
                vm.Genres = _context.Genres.ToList();
                return View("GigForm", vm);
            }
            var model = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = vm.GetDateTime(),
                Venue = vm.Venue,
                GenreId = vm.Genre
            };
            _context.Gigs.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }


        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);
            var vm = new GigFormViewModel
            {
                Heading = "Edit a Gig",
                Genres = _context.Genres.ToList(),
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
                vm.Genres = _context.Genres.ToList();
                return View("GigForm", vm);
            }

            var userId = User.Identity.GetUserId();

            var gig = _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == vm.Id && g.ArtistId == userId);

            gig.Modify(vm.Venue, vm.GetDateTime(), vm.Genre);

            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            var gigs = _context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();

            var attendances = _context.Attendances
                .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                .ToList()
                .ToLookup(a => a.GigId);

            var gigViewModel = new GigViewModel
            {
                Gigs = gigs,
                ShowAction = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Going",
                Attendances = attendances
            };

            return View("Gigs", gigViewModel);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Gigs
                .Where(g => g.ArtistId == userId && g.DateTime > DateTime.Now && !g.IsCanceled)
                .Include(g => g.Genre)
                .ToList();
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

            var gig = _context.Gigs
                //.Where(g => g.Id == id)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                //.Include(g => g.Attendances)
                //.Include(g => g.Artist.Followers)
                .SingleOrDefault(g => g.Id == id);

            if (gig == null)
                return HttpNotFound();

            var gigDetailViewModel = new GigDetailViewModel
            {
                Gig = gig
            };

            //var isAttending = false;
            //if (gig.Attendances.Where(a=>a.AttendeeId == userId).Count() > 0)
            //    isAttending = true;

            //var isFollowing = false;
            //if (gig.Artist.Followers.Where(f=>f.FollowerId == userId).Count() > 0)
            //    isFollowing = true;

            if (User.Identity.IsAuthenticated)
            {
                gigDetailViewModel.IsAttending = _context.Attendances.Any(a => a.GigId == gig.Id && a.AttendeeId == userId);
                gigDetailViewModel.IsFollowing = _context.Followings.Any(f => f.FolloweeId == gig.ArtistId && f.FollowerId == userId);
            }



            return View("Details", gigDetailViewModel);
        }
    }
}