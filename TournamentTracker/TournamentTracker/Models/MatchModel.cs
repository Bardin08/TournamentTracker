using System.Collections.Generic;

namespace TournamentTracker.Models
{
    public class MatchModel
    {
        /// <summary>
        /// Represents match teams with some additional information. Check also a <seealso cref="Models.MatchEntryModel"/>.
        /// </summary>
        public List<MatchEntryModel> Entries { get; set; } = new List<MatchEntryModel>();
        /// <summary>
        /// Represents the winner of this match.
        /// </summary>
        public TeamModel Winner { get; set; }
        /// <summary>
        /// Represents the round number.
        /// </summary>
        public int RoundNumber { get; set; }
    }
}