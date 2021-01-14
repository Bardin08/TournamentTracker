using TournamentTracker.Models;

namespace TournamentTracker.Interfaces
{
    public interface IDataConnection
    {
        PrizeModel CreatePrize(PrizeModel prize);        
    }
}
