using System.Collections.Generic;

namespace TournamentTracker.Data.DTOs
{
    public class MatchDto
    {
        /// <summary>
        /// Represents the unique identifier for a match model.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the winner unique identifier.
        /// </summary>
        public int? WinnerId { get; set; }

        /// <summary>
        /// Represents match teams with some additional information. Check also a <seealso cref="Models.MatchEntryModel"/>.
        /// </summary>
        public List<MatchEntryDto> Entries { get; set; } = new List<MatchEntryDto>();

        /// <summary>
        /// Represents the winner of this match.
        /// </summary>
        public int RoundNumber { get; set; }
    }
}