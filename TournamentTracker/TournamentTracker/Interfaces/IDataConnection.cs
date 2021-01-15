using TournamentTracker.Models;

namespace TournamentTracker.Interfaces
{
    /// <summary>
    /// Interface for data storage connectors. Contains methods to manipulate data.
    /// </summary>
    public interface IDataConnection
    {
        /// <summary>
        /// Allows to add the prize to the data storage.
        /// </summary>
        /// <param name="prize"> The prize model. </param>
        /// <returns></returns>
        PrizeModel CreatePrize(PrizeModel prize);        
    }
}
