using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IFollowingRepository
    {
        void Add(Following following);
        Following GetFollowing(string followeeId, string followerId);
        void Remove(Following following);
    }
}