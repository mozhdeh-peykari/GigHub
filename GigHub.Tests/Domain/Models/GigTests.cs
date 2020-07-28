using System;
using System.Linq;
using FluentAssertions;
using GigHub.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GigHub.Tests.Domain.Models
{
    [TestClass]
    public class GigTests
    {
        [TestMethod]
        public void Cancel_WhenCancelled_ShouldSetIsCanceledToTrue()
        {
            var gig = new Gig();
            gig.Cancel();
            gig.IsCanceled.Should().BeTrue();
        }
        [TestMethod]
        public void Cancel_WhenCancelled_EachAttendeeShouldhaveANotification()
        {
            var gig = new Gig();
            gig.Attendances.Add(new Attendance { Attendee = new ApplicationUser { Id="1"} });
            gig.Cancel();
            var attendees = gig.Attendances.Select(a => a.Attendee).ToList();
            attendees[0].UserNotifications.Count.Should().Be(1);
        }
    }
}
