using System.Collections.Generic;

using TournamentTracker.Data.DTOs;
using TournamentTracker.Domain.Models;

namespace TournamentTracker.Data.Mappings
{
    public static class PersonDtoToModel
    {
        public static PersonModel Map(PersonDto dto)
        {
            return new PersonModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CellphoneNumber = dto.CellphoneNumber,
                EmailAddress = dto.EmailAddress
            };
        }

        public static IEnumerable<PersonModel> Map(IEnumerable<PersonDto> dtos)
        {
            if (dtos is null)
            {
                throw new System.ArgumentNullException(nameof(dtos));
            }

            var output = new List<PersonModel>();

            foreach (var dto in dtos)
            {
                output.Add(Map(dto));
            }

            return output;
        }
    }
}
