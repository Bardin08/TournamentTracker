using TournamentTracker.Models;

namespace TournamentTrackerWPFUI.Interfaces
{
    public interface ITournamentRequester
    {
        void TournamentCreated(TournamentModel tournament);
    }
}
