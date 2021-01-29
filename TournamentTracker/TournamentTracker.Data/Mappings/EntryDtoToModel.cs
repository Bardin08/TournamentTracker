using System.Collections.Generic;
using System.Linq;

using TournamentTracker.Data.DTOs;
using TournamentTracker.Domain.Models;

namespace TournamentTracker.Data.Mappings
{
    public static class EntryDtoToModel
    {
        // required parent match always will be in the list, cause we process them from first round to last
        public static MatchEntryModel Map(IEnumerable<MatchModel> matches, IEnumerable<TeamModel> teams, MatchEntryDto dto)
        {
            return new MatchEntryModel
            {
                Id = dto.Id,
                ParentMatch = matches.FirstOrDefault(m => m.Id == dto.ParentMatchId),
                CompetingTeam = teams.FirstOrDefault(t => t.Id == dto.TeamCompetingId),
                Score = dto.Score
            };
        }

        public static IEnumerable<MatchEntryModel> Map(IEnumerable<MatchModel> matches, IEnumerable<TeamModel> teams, IEnumerable<MatchEntryDto> dtos)
        {
            var output = new List<MatchEntryModel>();

            foreach (var dto in dtos)
            {
                output.Add(Map(matches, teams, dto));
            }

            return output;
        }
    }
}
