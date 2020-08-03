using System;
using System.Data.Entity;
using FluentAssertions;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Persistence;
using GigHub.Persistence.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;

namespace GigHub.Tests.Persistence.Repositories
{
    [TestClass]
    public class GigRepositoryTests
    {
        private GigRepository _repository;
        private Mock<DbSet<Gig>> _mockDbsetGig;
        private Mock<DbSet<Attendance>> _mockDbSetAttendance;

        [TestInitialize]
        public void TestInitialize()
        {
            var mockContext = new Mock<IApplicationDbContext>();
            _repository = new GigRepository(mockContext.Object);
            _mockDbsetGig = new Mock<DbSet<Gig>>();
            _mockDbSetAttendance = new Mock<DbSet<Attendance>>();
            mockContext.SetupGet(c => c.Gigs).Returns(_mockDbsetGig.Object);
            mockContext.SetupGet(c => c.Attendances).Returns(_mockDbSetAttendance.Object);
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsInThePast_ShouldNotBeReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(-1), ArtistId="1" };
            _mockDbsetGig.SetSource(new[] { gig });
            var result =_repository.GetUpcomingGigsByArtist("1");
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsCancelled_ShouldNotBeReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };
            gig.Cancel();
            _mockDbsetGig.SetSource(new[] { gig });
            var result = _repository.GetUpcomingGigsByArtist("1");
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsForAnotherArtist_ShouldNotBeReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };
            _mockDbsetGig.SetSource(new[] { gig });
            var result = _repository.GetUpcomingGigsByArtist(gig.ArtistId+"-");
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsForTheFutureAndForTheGivenArtist_ShouldBeReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };
            _mockDbsetGig.SetSource(new[] { gig });
            var result = _repository.GetUpcomingGigsByArtist(gig.ArtistId);
            result.Should().Contain(gig);
        }

        [TestMethod]
        public void GetGigsUserAttending_AttendanceForAnotherUser_ShouldNotBeReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(1) };
            var attendance = new Attendance { Gig = gig, AttendeeId = "1" };
            _mockDbSetAttendance.SetSource(new[] { attendance });
            var result = _repository.GetGigsUserAttending(attendance.AttendeeId+"-");
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetGigsUserAttending_GigIsInThePast_ShouldNotBeReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(-1) };
            var attendance = new Attendance { Gig = gig, AttendeeId = "1" };
            _mockDbSetAttendance.SetSource(new[] { attendance });
            var result = _repository.GetGigsUserAttending(attendance.AttendeeId);
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetGigsUserAttending_GigIsInTheFutureAndAttendanceForTheGivenUser_ShouldBeReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(1) };
            var attendance = new Attendance { Gig = gig, AttendeeId = "1" };
            _mockDbSetAttendance.SetSource(new[] { attendance });
            var result = _repository.GetGigsUserAttending(attendance.AttendeeId);
            result.Should().Contain(gig);
        }
    }
}
