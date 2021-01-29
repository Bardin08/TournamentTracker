using System.Collections.Generic;

namespace TournamentTracker.Data.DTOs
{
    public class TeamDto
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
        public List<PersonDto> TeamMembers { get; set; } = new List<PersonDto>();
    }
}