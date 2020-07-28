using System;
using System.Web.Http.Results;
using FluentAssertions;
using GigHub.Controllers.Api;
using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GigHub.Tests.Controllers.Api
{
    [TestClass]
    public class AttendancesControllerTests
    {
        private string _userId;
        private AttendancesController _controller;
        private Mock<IAttendanceRepository> _mockAttendanceRepository;
        [TestInitialize]
        public void TestInitialize()
        {
            _userId = "1";
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new AttendancesController(mockUnitOfWork.Object);
            _controller.MockCurrentUser(_userId, "user1@domain.com");

            _mockAttendanceRepository = new Mock<IAttendanceRepository>();
            mockUnitOfWork.SetupGet(u => u.Attendances).Returns(_mockAttendanceRepository.Object);

        }

        [TestMethod]
        public void Attend_AttendanceAlreadyExists_ShouldReturnBadRequest()
        {
            var attendance = new Attendance();
            _mockAttendanceRepository.Setup(r => r.GetAttendance(1, _userId)).Returns(attendance);
            var result = _controller.Attend(new AttendanceDto { GigId = 1 });
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public void Attend_ValidRequest_ShouldReturnOk()
        {
            var result = _controller.Attend(new AttendanceDto { GigId = 1 });
            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public void DeleteAttendance_NotExists_ShouldReturnNotFound()
        {
            var result = _controller.DeleteAttendance(1);
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void DeleteAttendance_ValidRequest_ShouldReturnOk()
        {
            var attendance = new Attendance();
            _mockAttendanceRepository.Setup(r => r.GetAttendance(1, _userId)).Returns(attendance);
            var result = _controller.DeleteAttendance(1);
            result.Should().BeOfType<OkNegotiatedContentResult<int>>();
        }

        [TestMethod]
        public void DeleteAttendance_ValidRequest_ShouldReturnIdOfDeletedAttendance()
        {
            var attendance = new Attendance();
            _mockAttendanceRepository.Setup(r => r.GetAttendance(1, _userId)).Returns(attendance);
            var result = (OkNegotiatedContentResult<int>) _controller.DeleteAttendance(1);
            result.Content.Should().Be(1);
        }
    }
}
