var FollowingService = function () {
    var createFollowing = function (artistId, doneFollowing, fail) {
        $.post("/api/followings", { followeeId: artistId })
            .done(doneFollowing)
            .fail(fail);
    };

    var deleteFollowing = function (artistId, doneFollowing, fail) {
        $.ajax({
            url: "/api/followings/" + artistId,
            method: "DELETE"
        })
            .done(doneFollowing)
            .fail(fail);
    };

    return {
        createFollowing: createFollowing,
        deleteFollowing: deleteFollowing

    }
}();

