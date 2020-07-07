using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GigHub.Core.Models
{
    public class Gig
    {
        public int Id { get; set; }

        [Required]
        public string ArtistId { get; set; }
        public ApplicationUser Artist { get; set; }

        public DateTime DateTime { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        [Required]
        public byte GenreId { get; set; }
        public Genre Genre { get; set; }

        public bool IsCanceled { get; private set; }

        public ICollection<Attendance> Attendances { get; private set; }

        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }

        public void Cancel()
        {
            IsCanceled = true;

            //notification
            var notification = Notification.GigCanceled(this);

            foreach (var attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
            //
        }

        public void Modify(string venue, DateTime dateTime, byte genre)
        {
            //Notification
            var notification = Notification.GigUpdated(this, dateTime, venue);

            //Update
            Venue = venue;
            DateTime = dateTime;
            GenreId = genre;

            //UserNotification
            foreach(var attendee in this.Attendances.Select(a=>a.Attendee))
            {
                attendee.Notify(notification);
            }
        }
    }
}