using GigHub.Core.Dtos;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.Core.Models;
using GigHub.Persistence;
using GigHub.Core.Repositories;
using GigHub.Core;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public FollowingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowingDto followingDto)
        {
            var userId = User.Identity.GetUserId();
            var exists = _unitOfWork.Followings.GetFollowing(followingDto.FolloweeId, userId);
                //_context.Followings.Any(f => f.FolloweeId == followingDto.FolloweeId && f.FollowerId == userId);

            if (exists != null)
            {
                return BadRequest("Already following");
            }

            var model = new Following
            {
                FolloweeId = followingDto.FolloweeId,
                FollowerId = userId
            };

            _unitOfWork.Followings.Add(model);
            _unitOfWork.Complete();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult UnFollow(string id)
        {
            var userId = User.Identity.GetUserId();
            var follow =  _unitOfWork.Followings.GetFollowing(id, userId);

            if (follow == null)
                return NotFound();

            _unitOfWork.Followings.Remove(follow);
            _unitOfWork.Complete();

            return Ok(id);
        }
    }
}
