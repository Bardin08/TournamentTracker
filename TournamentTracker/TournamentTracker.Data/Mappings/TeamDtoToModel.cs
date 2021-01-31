using System.Collections.Generic;
using System.Linq;

using TournamentTracker.Data.DTOs;
using TournamentTracker.Domain.Models;

namespace TournamentTracker.Data.Mappings
{
    public static class TeamDtoToModel
    {
        public static TeamModel Map(TeamDto dto)
        {
            return new TeamModel
            {
                Id = dto.Id,
                TeamName = dto.TeamName,
                TeamMembers = PersonDtoToModel.Map(dto.TeamMembers).ToList()
            };
        }

        public static IEnumerable<TeamModel> Map(IEnumerable<TeamDto> dtos)
        {
            if (dtos is null)
            {
                throw new System.ArgumentNullException(nameof(dtos));
            }

            var output = new List<TeamModel>();

            foreach (var dto in dtos)
            {
                output.Add(Map(dto));
            }

            return output;
        }
    }
}
