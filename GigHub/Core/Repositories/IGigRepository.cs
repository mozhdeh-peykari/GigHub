using System.Collections.Generic;
using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IGigRepository
    {
        void AddGig(Gig gig);
        Gig GetGig(int id);
        IEnumerable<Gig> GetGigsUserAttending(string userId);
        Gig GetGigWithAttendees(int gigId);
        IEnumerable<Gig> GetUpComingGigs();
        IEnumerable<Gig> GetUpcomingGigsByArtist(string artistId);
    }
}