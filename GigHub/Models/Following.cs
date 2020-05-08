using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GigHub.Models
{
    public class Following
    {
        public ApplicationUser Followee { get; set; }
        public ApplicationUser Follower { get; set; }
        [Key]
        [Column(Order = 1)]
        public string FolloweeId { get; set; }
        [Key]
        [Column(Order = 2)]
        public string FollowerId { get; set; }
    }
}