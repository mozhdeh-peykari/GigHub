using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Core.Models
{
    public class Following
    {
        public ApplicationUser Followee { get; set; }
        public ApplicationUser Follower { get; set; }

        public string FolloweeId { get; set; }

        public string FollowerId { get; set; }
    }
}