namespace TournamentTracker.Models
{
    public class MatchEntryModel
    {
        /// <summary>
        /// Represents one team in the match.
        /// </summary>
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