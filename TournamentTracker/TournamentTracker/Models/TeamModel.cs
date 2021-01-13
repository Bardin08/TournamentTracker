using System.Collections.Generic;

namespace TournamentTracker.Models
{
    public class TeamModel
    {
        /// <summary>
        /// Represents the team name.
        /// </summary>
        public string TeamName { get; set; }
        /// <summary>
        /// Represents the team members.
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();
    }
}
