using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TournamentTracker.Data.DTOs;
using TournamentTracker.Domain.Models;

namespace TournamentTracker.Data.Mappings
{
    public class PrizeDtoToModel
    {
        public static PrizeModel Map(PrizeDto dto)
        {
            return new PrizeModel
            {
                Id = dto.Id,
                PlaceNumber = dto.PlaceNumber,
                PlaceName = dto.PlaceName,
                PrizeAmount = dto.PrizeAmount,
                PrizeName = dto.PrizeName,
                PrizePercentage = dto.PrizePercentage
            };
        }

        public static IEnumerable<PrizeModel> Map(IEnumerable<PrizeDto> dtos)
        {
            if (dtos is null)
            {
                throw new System.ArgumentNullException(nameof(dtos));
            }

            var output = new List<PrizeModel>();

            foreach (var dto in dtos)
            {
                output.Add(Map(dto));
            }

            return output;
        }
    }
}
