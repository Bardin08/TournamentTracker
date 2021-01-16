using System.Collections.Generic;

namespace TournamentTracker.Models
{
    public class TeamModel
    {
        /// <summary>
        /// Represents team unique identifier.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Represents the team name.
        /// </summary>
        public string TeamName { get; set; }
        /// <summary>
        /// Represents the team members.
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();

        /// <summary>
        /// Constructs a team model.
        /// </summary>
        public TeamModel()
        {
        }

        /// <summary>
        /// Constructs a team model.
        /// </summary>
        /// <param name="teamName"> Team name. </param>
        /// <param name="teamMembers"> A list of person models which represents team members. </param>
        public TeamModel(string teamName, List<PersonModel> teamMembers)
        {
            TeamName = teamName ?? throw new System.ArgumentNullException(nameof(teamMembers));
            TeamMembers = teamMembers ?? throw new System.ArgumentNullException(nameof(teamName));
        }
    }
}
