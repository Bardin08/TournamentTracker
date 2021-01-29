namespace TournamentTracker.Domain.Models
{
    public class MatchEntryModel
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
        /// Represents one team in the match.
        /// </summary
        public TeamModel CompetingTeam { get; set; }
        /// <summary>
        /// Represents the score for this particular team.
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// Represents the match that this team came from as the winner.
        /// </summary>
        public MatchModel ParentMatch { get; set; }
    }
}