using System.Collections.Generic;
using System.Linq;

using TournamentTracker.Data.DTOs;
using TournamentTracker.Domain.Models;

namespace TournamentTracker.Data.Mappings
{
    public static class TournamentDtoToModel
    {
        public static TournamentModel Map(TournamentDto dto)
        {
            var model = new TournamentModel
            {
                Id = dto.Id,
                EntryFee = dto.EntryFee,
                TournamentName = dto.TournamentName,
                EnteredTeams = TeamDtoToModel.Map(dto.EnteredTeams).ToList(),
                Prizes = PrizeDtoToModel.Map(dto.Prizes).ToList(),
                Rounds = new List<List<MatchModel>>()
            };

            foreach (var r in dto.Rounds)
            {
                model.Rounds.Add(MatchDtoToModel.Map(model.EnteredTeams, r).ToList());
            }

            return model;
        }
    
        public static IEnumerable<TournamentModel> Map(IEnumerable<TournamentDto> dtos)
        {
            if (dtos is null)
            {
                throw new System.ArgumentNullException(nameof(dtos));
            }

            var output = new List<TournamentModel>();

            foreach (var dto in dtos)
            {
                output.Add(Map(dto));
            }

            return output;
        }
    }
}
