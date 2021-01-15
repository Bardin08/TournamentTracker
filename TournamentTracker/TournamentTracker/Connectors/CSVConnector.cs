using System.Linq;
using System.Collections.Generic;
using TournamentTracker.Models;
using TournamentTracker.Interfaces;
using TournamentTracker.Connectors.TextHelpers;

namespace TournamentTracker.Connectors
{
    /// <summary>
    /// Connector for CSV file. Allows saving data to CSV files.
    /// </summary>
    public class CSVConnector : IDataConnection
    {
        public const string PrizesFilePath = "PrizeModels.csv";
        public const string PeopleFilePath = "PeopleModels.csv";

        public PersonModel CreatePerson(PersonModel person)
        {
            List<PersonModel> people = PeopleFilePath.GetFilePath().LoadFile().ConvertLinesToPersonModels();

            var lastId = 0;

            if (people.Count > 0)
            {
                lastId = people.OrderByDescending(x => x.Id).First().Id;
            }

            person.Id = lastId + 1;

            people.Add(person);
            people.SaveToPeopleFile(PeopleFilePath);

            return person;
        }

        public PrizeModel CreatePrize(PrizeModel prize)
        {
            List<PrizeModel> prizes = PrizesFilePath.GetFilePath().LoadFile().ConvertLinesToPrizeModels();
            var lastPrizeId = 0;
            
            if (prizes.Count > 0)
            {
                lastPrizeId = prizes.OrderByDescending(x => x.Id).First().Id;
            }

            prize.Id = lastPrizeId + 1;

            prizes.Add(prize);
            prizes.SaveToPrizesFile(PrizesFilePath);

            return prize;
        }
    }
}
