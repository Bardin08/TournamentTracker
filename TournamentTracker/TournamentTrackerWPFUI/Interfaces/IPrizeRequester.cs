using TournamentTracker.Models;

namespace TournamentTrackerWPFUI.Interfaces
{
    public interface IPrizeRequester
    {
        void PrizeCreated(PrizeModel prize);        
    }
}
