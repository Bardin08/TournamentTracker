namespace TournamentTracker.Data.DTOs
{
    public class MatchEntryDto
    {
        /// <summary>
        /// Represents the unique identifier for match entry model.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the unique identifier of a team in the match.
        /// </summary
        public int? TeamCompetingId { get; set; }

        /// <summary>
        /// Represents the match that this team came from as the winner.
        /// </summary>
        public int ParentMatchId { get; set; }

        /// <summary>
        /// Represents one team in the match.
        /// </summary
        public double Score { get; set; }
    }
}