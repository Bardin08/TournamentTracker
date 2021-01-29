using System.Collections.Generic;

namespace TournamentTracker.Domain.Models
{
    public class TournamentModel
    {
        /// <summary>
        /// Represents tournament unique identifier.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Represents the tournament name.
        /// </summary>
        public string TournamentName { get; set; }
        /// <summary>
        /// Represents the tournament entry fee.
        /// </summary>
        public decimal EntryFee { get; set; }
        /// <summary>
        /// Represents the tournament teams.
        /// </summary>
        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();
        /// <summary>
        /// Represents the tournament prizes.
        /// </summary>
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();
        /// <summary>
        /// Represents the tournament rounds.
        /// </summary>
        public List<List<MatchModel>> Rounds { get; set; } = new List<List<MatchModel>>();
    }
}
