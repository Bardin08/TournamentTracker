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
        private const string PrizesFileName = "PrizeModels.csv";
        private const string PeopleFileName = "PeopleModels.csv";
        private const string TeamsFileName = "TeamModels.csv";

        public PersonModel CreatePerson(PersonModel person)
        {
            List<PersonModel> people = PeopleFileName.GetFilePath().LoadFile().ConvertLinesToPersonModels();

            var lastId = 0;

            if (people.Count > 0)
            {
                lastId = people.OrderByDescending(x => x.Id).First().Id;
            }

            person.Id = lastId + 1;

            people.Add(person);
            people.SaveToPeopleFile(PeopleFileName);

            return person;
        }

        public PrizeModel CreatePrize(PrizeModel prize)
        {
            List<PrizeModel> prizes = PrizesFileName.GetFilePath().LoadFile().ConvertLinesToPrizeModels();
            var lastPrizeId = 0;
            
            if (prizes.Count > 0)
            {
                lastPrizeId = prizes.OrderByDescending(x => x.Id).First().Id;
            }

            prize.Id = lastPrizeId + 1;

            prizes.Add(prize);
            prizes.SaveToPrizesFile(PrizesFileName);

            return prize;
        }

        public TeamModel CreateTeam(TeamModel team)
        {
            var teams = TeamsFileName.GetFilePath().LoadFile().ConvertLinesToTeamModels(PeopleFileName);

            int lastId = 0;

            if (teams.Count > 0)
            {
                lastId = teams.OrderByDescending(x => x.Id).First().Id;
            }

            team.Id = lastId + 1;

            teams.Add(team);
            teams.SaveToTeamsFile(TeamsFileName);

            return team;
        }

        public List<PersonModel> GetAllParticipants()
        {
            return PeopleFileName.GetFilePath().LoadFile().ConvertLinesToPersonModels();
        }
    }
}
