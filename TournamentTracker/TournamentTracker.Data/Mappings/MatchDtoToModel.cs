using System.Collections.Generic;
using System.Linq;

using TournamentTracker.Data.DTOs;
using TournamentTracker.Domain.Models;

namespace TournamentTracker.Data.Mappings
{
    public static class MatchDtoToModel
    {
        public static MatchModel Map(IEnumerable<MatchModel> matches, IEnumerable<TeamModel> teams, MatchDto dto)
        {
            return new MatchModel
            {
                Id = dto.Id,
                RoundNumber = dto.RoundNumber,
                Winner = teams.FirstOrDefault(t => t.Id == dto.WinnerId),
                Entries = EntryDtoToModel.Map(matches, teams, dto.Entries).ToList()
            };
        }

        public static IEnumerable<MatchModel> Map(IEnumerable<TeamModel> teams, IEnumerable<MatchDto> dtos)
        {
            if (dtos is null)
            {
                throw new System.ArgumentNullException(nameof(dtos));
            }

            var output = new List<MatchModel>();

            foreach (var dto in dtos)
            {
                output.Add(Map(output, teams, dto));
            }

            return output;
        }
    }
}
