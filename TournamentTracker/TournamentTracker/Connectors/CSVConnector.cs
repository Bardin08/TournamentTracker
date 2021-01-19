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
        public PersonModel CreatePerson(PersonModel person)
        {
            List<PersonModel> people = GlobalConfiguration.PeopleFileName
                .GetFilePath()
                .LoadFile()
                .ToPersonModels();

            var lastSavedId = 0;
            if (people.Count > 0)
            {
                lastSavedId = people.OrderByDescending(x => x.Id).First().Id;
            }
            person.Id = lastSavedId + 1;

            people.Add(person);
            people.SaveToPeopleFile();

            return person;
        }

        public PrizeModel CreatePrize(PrizeModel prize)
        {
            List<PrizeModel> prizes = GlobalConfiguration.PrizesFileName
                .GetFilePath()
                .LoadFile()
                .ToPrizeModels();

            var lastSavedId = 0;
            if (prizes.Count > 0)
            {
                lastSavedId = prizes.OrderByDescending(x => x.Id).First().Id;
            }
            prize.Id = lastSavedId + 1;

            prizes.Add(prize);
            prizes.SaveToPrizesFile();

            return prize;
        }

        public TeamModel CreateTeam(TeamModel team)
        {
            var teams = GlobalConfiguration.TeamsFileName
                .GetFilePath()
                .LoadFile()
                .ToTeamModels();

            int lastId = 0;

            if (teams.Count > 0)
            {
                lastId = teams.OrderByDescending(x => x.Id).First().Id;
            }

            team.Id = lastId + 1;

            teams.Add(team);
            teams.SaveToTeamsFile();

            return team;
        }

        public TournamentModel CreateTournament(TournamentModel tournament)
        {
            var tournaments = GlobalConfiguration.TournamentsFileName
                .GetFilePath()
                .LoadFile()
                .ToTournamentModels();

            int lastId = 0;
            if (tournaments.Count > 0)
            {
                lastId = tournaments.OrderByDescending(x => x.Id).First().Id;
            }
            tournament.Id = lastId + 1;

            tournament.SaveRoundsToFile();

            tournaments.Add(tournament);
            tournaments.SaveToTournamentsFile();

            return tournament;
        }

        public List<PersonModel> GetAllParticipants()
        {
            return GlobalConfiguration.PeopleFileName
                .GetFilePath()
                .LoadFile()
                .ToPersonModels();
        }

        public List<PrizeModel> GetPrizes()
        {
            return GlobalConfiguration.PrizesFileName
                .GetFilePath()
                .LoadFile()
                .ToPrizeModels();
        }

        public List<TeamModel> GetTeams()
        {
            return GlobalConfiguration.TeamsFileName
                .GetFilePath()
                .LoadFile()
                .ToTeamModels();
        }
    }
}
