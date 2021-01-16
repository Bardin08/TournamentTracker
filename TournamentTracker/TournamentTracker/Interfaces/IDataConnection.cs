using System.Collections.Generic;
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
        /// <returns> Updated prize model with unique identifier. </returns>
        PrizeModel CreatePrize(PrizeModel prize);

        /// <summary>
        /// Allows to add the person to the data storage.
        /// </summary>
        /// <param name="person"> The person model. </param>
        /// <returns> Updated person model with unique identifier.  </returns>
        PersonModel CreatePerson(PersonModel person);

        /// <summary>
        /// Allows to add a team to the data storage.
        /// </summary>
        /// <param name="team"> The team model. </param>
        /// <returns> Updated team model with unique identifier. </returns>
        TeamModel CreateTeam(TeamModel team);

        /// <summary>
        /// Allows to get all participants data from the data storage.
        /// </summary>
        /// <returns> The collection of person model. </returns>
        List<PersonModel> GetAllParticipants();
    }
}
