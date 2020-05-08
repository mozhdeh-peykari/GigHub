﻿using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigHub.ViewModels
{
    public class GigViewModel
    {
        public IEnumerable<Gig> Gigs { get; set; }
        public bool ShowAction { get; set; }
        public string Heading { get; set; }
    }
}