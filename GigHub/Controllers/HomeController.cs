using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Core.ViewModels;
using GigHub.Persistence;
using GigHub.Persistence.Repositories;
using GigHub.Core;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index(string query = null)
        {
            var userId = User.Identity.GetUserId();

            var gigs = _unitOfWork.Gigs.GetUpComingGigs();

            if (!String.IsNullOrWhiteSpace(query))
            {
                gigs = gigs.Where(g => g.Artist.Name.Contains(query) ||
                                     g.Genre.Name.Contains(query) ||
                                     g.Venue.Contains(query));
            }

            var attendances = _unitOfWork.Attendances.GetFutureAttendances(userId).ToLookup(a => a.GigId);

            var homeViewModel = new GigViewModel
            {
                Gigs = gigs,
                ShowAction = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs",
                SearchTerm = query,
                Attendances = attendances
            };
            return View("Gigs", homeViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}