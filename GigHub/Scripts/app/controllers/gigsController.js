var GigsController = function (attendanceService) {

    var button;

    var init = function (container) {
        $(container).on("click", ".js-toggle-attendance", toggleAttendance);
    };

    var toggleAttendance = function (e) {

        button = $(e.target);
        var gigId = button.attr("data-gig-id");

        if (button.hasClass("btn-default"))
            attendanceService.createAttendance(gigId, done, fail);
        else
            attendanceService.deleteAttendance(gigId, done, fail);
    };

    var done = function () {
        var text = (button.text() == "Going") ? "Going?" : "Going";
        button.toggleClass("btn-default").toggleClass("btn-info").text(text);
    };

    var fail = function () {
        alert("Something failed!");
    };

    return { init: init }
}(AttendanceService);
