using GigHub.Dtos;
using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigHub.ViewModels
{
    public class GigDetailViewModel
    {
        public Gig Gig { get; set; }
        //public bool ShowAction { get; set; }
        public bool IsAttending { get; set; }
        public bool IsFollowing { get; set; }
    }
}