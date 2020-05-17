using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private ApplicationDbContext _context;
        public FollowingsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowingDto followingDto)
        {
            var userId = User.Identity.GetUserId();
            var exists = _context.Followings.Any(f => f.FolloweeId == followingDto.FolloweeId && f.FollowerId == userId);

            if (exists)
            {
                return BadRequest("Already following");
            }

            var model = new Following
            {
                FolloweeId = followingDto.FolloweeId,
                FollowerId = userId
            };

            _context.Followings.Add(model);
            _context.SaveChanges();

            return Ok();
        }
    }
}
