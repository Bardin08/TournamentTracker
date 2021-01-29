using System.Collections.Generic;

namespace TournamentTracker.Data.DTOs
{
    public class TournamentDto
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
        public List<TeamDto> EnteredTeams { get; set; } = new List<TeamDto>();

        /// <summary>
        /// Represents the tournament prizes.
        /// </summary>
        public List<PrizeDto> Prizes { get; set; } = new List<PrizeDto>();

        /// <summary>
        /// Represents the tournament rounds.
        /// </summary>
        public List<List<MatchDto>> Rounds { get; set; } = new List<List<MatchDto>>();
    }
}
