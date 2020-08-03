using System;
using System.Data.Entity;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Common;
using GigHub.Core.Models;
using GigHub.Persistence;
using GigHub.Persistence.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GigHub.Tests.Persistence.Repositories
{
    [TestClass]
    public class NotificationRepositoryTests
    {
        private NotificationRepository _repository;
        private Mock<DbSet<UserNotification>> _mockDbsetUserNotification;

        [TestInitialize]
        public void TestInitialize()
        {
            var _mockDbContext = new Mock<IApplicationDbContext>();
            _mockDbsetUserNotification = new Mock<DbSet<UserNotification>>();
            _mockDbContext.SetupGet(c => c.UserNotifications).Returns(_mockDbsetUserNotification.Object);
            _repository = new NotificationRepository(_mockDbContext.Object);
        }

        [TestMethod]
        public void GetNewNotificationsFor_IsRead_ShouldNotBeReturned()
        {
            var user = new ApplicationUser { Id = "1" };
            var notification = Notification.GigCanceled(new Gig());
            var userNotification = new UserNotification(user, notification);
            userNotification.Read();

            _mockDbsetUserNotification.SetSource(new[] { userNotification });

            var result = _repository.GetNewNotificationsFor(user.Id);

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetNewNotificationsFor_IsForAnotherUser_ShouldNotBeReturned()
        {
            var user = new ApplicationUser { Id = "1" };
            var notification = Notification.GigCanceled(new Gig());
            var userNotification = new UserNotification(user, notification);

            _mockDbsetUserNotification.SetSource(new[] { userNotification });

            var result = _repository.GetNewNotificationsFor(user.Id + "-");

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetNewNotificationsFor_IsNewAndForGivenUser_ShouldBeReturned()
        {
            var user = new ApplicationUser { Id = "1" };
            var notification = Notification.GigCanceled(new Gig());
            var userNotification = new UserNotification(user, notification);

            _mockDbsetUserNotification.SetSource(new[] { userNotification });

            var result = _repository.GetNewNotificationsFor(user.Id);

            //result.Should().Contain(notification);
            result.Should().HaveCount(1);
            result.First().Should().Be(notification);
        }
    }
}
