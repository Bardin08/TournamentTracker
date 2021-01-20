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
        PrizeModel SavePrize(PrizeModel prize);

        /// <summary>
        /// Allows to add the person to the data storage.
        /// </summary>
        /// <param name="person"> The person model. </param>
        /// <returns> Updated person model with unique identifier.  </returns>
        PersonModel SavePerson(PersonModel person);

        /// <summary>
        /// Allows to add a team to the data storage.
        /// </summary>
        /// <param name="team"> The team model. </param>
        /// <returns> Updated team model with unique identifier. </returns>
        TeamModel SaveTeam(TeamModel team);

        /// <summary>
        /// Allows to save the tournament model to the data storage. 
        /// </summary>
        /// <param name="tournament"> The tournament model. </param>
        /// <returns> Updated tournament model with unique identifier. </returns>
        TournamentModel SaveTournament(TournamentModel tournament);

        /// <summary>
        /// Allows to get all participants data from the data storage.
        /// </summary>
        /// <returns> The collection of <seealso cref="PersonModel"/>. </returns>
        List<PersonModel> GetAllParticipants();

        /// <summary>
        /// Allows to get all teams from the data storage.
        /// </summary>
        /// <returns> The collection of <seealso cref="TeamModel"/>. </returns>
        List<TeamModel> GetTeams();

        /// <summary>
        /// Allows to get all prizes from the data storage.
        /// </summary>
        /// <returns> The collection of <see cref="PrizeModel"/>. </returns>
        List<PrizeModel> GetPrizes();
    }
}
