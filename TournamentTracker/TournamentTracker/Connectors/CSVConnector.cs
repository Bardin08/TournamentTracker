using System.Linq;
using System.Collections.Generic;
using TournamentTracker.Models;
using TournamentTracker.Interfaces;
using TournamentTracker.Connectors.TextHelpers;
using System;

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
        private const string TournamentsFileName = "TournamentsModels.csv";

        public PersonModel CreatePerson(PersonModel person)
        {
            List<PersonModel> people = PeopleFileName.GetFilePath().LoadFile().ToPersonModels();

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
            List<PrizeModel> prizes = PrizesFileName.GetFilePath().LoadFile().ToPrizeModels();
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
            var teams = TeamsFileName.GetFilePath().LoadFile().ToTeamModels(PeopleFileName);

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

        public TournamentModel CreateTournament(TournamentModel tournament)
        {
            // TODO: Change 3-rd parameter from "" to matchesFileName
            var tournaments = TournamentsFileName.GetFilePath().LoadFile().ToTournamentModels(TeamsFileName, PrizesFileName, "", PeopleFileName);

            int lastId = 0;

            if (tournaments.Count > 0)
            {
                lastId = tournaments.OrderByDescending(x => x.Id).First().Id;
            }

            tournament.Id = lastId + 1;
            tournaments.Add(tournament);
            tournaments.SaveToTournamentsFile(TournamentsFileName, TeamsFileName, PrizesFileName, "");

            return tournament;
        }

        public List<PersonModel> GetAllParticipants()
        {
            return PeopleFileName.GetFilePath().LoadFile().ToPersonModels();
        }

        public List<PrizeModel> GetPrizes()
        {
            return PrizesFileName.GetFilePath().LoadFile().ToPrizeModels();
        }

        public List<TeamModel> GetTeams()
        {
            return TeamsFileName.GetFilePath().LoadFile().ToTeamModels(PeopleFileName);
        }
    }
}
