﻿using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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
                Genres = _context.Genres.ToList()
            };
            return View(vm);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Create(GigFormViewModel vm)
        {
            var model = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = vm.DateTime,
                Venue = vm.Venue,
                GenreId = vm.Genre
            };
            _context.Gigs.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}