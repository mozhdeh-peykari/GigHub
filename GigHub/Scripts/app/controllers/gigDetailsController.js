var GigDetailsController = function (followingService) {

    var button;

    var init = function () {
        $(".js-toggle-following").click(toggleFollowing);
    };

    var toggleFollowing = function (e) {

        button = $(e.target);
        var artistId = button.attr("data-artist-id");

        if (button.hasClass("btn-default"))
            followingService.createFollowing(artistId, done, fail);
        else
            followingService.deleteFollowing(artistId, done, fail);
    };

    var done = function () {
        var text = (button.text() == "Follow") ? "Following" : "Follow";
        button.toggleClass("btn-default").toggleClass("btn-info").text(text);
    };

    var fail = function () {
        alert("Something failed!");
    };

    return {
        init: init,
    }
}(FollowingService);
