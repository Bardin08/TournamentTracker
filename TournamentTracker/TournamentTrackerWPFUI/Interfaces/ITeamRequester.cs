using TournamentTracker.Domain.Models;

namespace TournamentTrackerWPFUI.Interfaces
{
    public interface ITeamRequester
    {
        void TeamCreated(TeamModel team);        
    }
}
