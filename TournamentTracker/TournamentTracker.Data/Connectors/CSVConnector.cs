﻿using System;
using System.Collections.Generic;
using System.Linq;
using TournamentTracker.Data.Connectors.TextHelpers;
using TournamentTracker.Data.Interfaces;
using TournamentTracker.Domain.Models;

namespace TournamentTracker.Data.Connectors
{
    /// <summary>
    /// Connector for CSV file. Allows saving data to CSV files.
    /// </summary>
    [Obsolete]
    public class CSVConnector : IDataConnection
    {
        #region Save data to the data storage

        public PersonModel SavePerson(PersonModel person)
        {
            List<PersonModel> people = DataConfiguration.PeopleFileName
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
            people.PeopleToLines()
                .SaveToFile(DataConfiguration.PeopleFileName.GetFilePath());

            return person;
        }

        public PrizeModel SavePrize(PrizeModel prize)
        {
            List<PrizeModel> prizes = DataConfiguration.PrizesFileName
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
            prizes.PrizesToLines()
                .SaveToFile(DataConfiguration.PrizesFileName.GetFilePath());

            return prize;
        }

        public TeamModel SaveTeam(TeamModel team)
        {
            var teams = DataConfiguration.TeamsFileName
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
            teams.TeamsToLines()
                .SaveToFile(DataConfiguration.TeamsFileName.GetFilePath());

            return team;
        }

        public TournamentModel SaveTournament(TournamentModel tournament)
        {
            var tournaments = DataConfiguration.TournamentsFileName
                .GetFilePath()
                .LoadFile()
                .ToTournamentModels();

            int lastId = 0;
            if (tournaments.Count > 0)
            {
                lastId = tournaments.OrderByDescending(x => x.Id).First().Id;
            }
            tournament.Id = lastId + 1;

            SaveRounds(tournament);

            tournaments.Add(tournament);
            tournaments.TournamentToLines()
                .SaveToFile(DataConfiguration.TournamentsFileName.GetFilePath());

            return tournament;
        }

        private void SaveRounds(TournamentModel tournament)
        {
            foreach (var round in tournament.Rounds)
            {
                foreach (var match in round)
                {
                    SaveMatch(match);
                }
            }
        }

        private void SaveMatch(MatchModel match)
        {
            List<MatchModel> matches = DataConfiguration.MatchesFileName
                .GetFilePath()
                .LoadFile()
                .ToMatchModels();

            int lastSavedId = 0;
            if (matches.Count > 0)
            {
                lastSavedId = matches.OrderByDescending(x => x.Id).First().Id;
            }
            match.Id = lastSavedId + 1;
            matches.Add(match);

            foreach (var entry in match.Entries)
            {
                SaveMatchEntry(entry);
            }

            matches.MatchesToLines()
                .SaveToFile(DataConfiguration.MatchesFileName.GetFilePath());
        }

        private void SaveMatchEntry(MatchEntryModel entry)
        {
            List<MatchEntryModel> entries = DataConfiguration.MatchEntriesFileName
                .GetFilePath()
                .LoadFile()
                .ToMatchEntryModels();

            int lastSavedId = 0;
            if (entries.Count > 0)
            {
                lastSavedId = entries.OrderByDescending(x => x.Id).First().Id;
            }
            entry.Id = lastSavedId + 1;
            
            entries.Add(entry);
            entries.MatchEntriesToLines()
                .SaveToFile(DataConfiguration.MatchEntriesFileName.GetFilePath());
        }

        #endregion

        #region Get data from the data storage

        public List<PersonModel> GetAllParticipants()
        {
            return DataConfiguration.PeopleFileName
                .GetFilePath()
                .LoadFile()
                .ToPersonModels();
        }

        public List<PrizeModel> GetPrizes()
        {
            return DataConfiguration.PrizesFileName
                .GetFilePath()
                .LoadFile()
                .ToPrizeModels();
        }

        public List<TeamModel> GetTeams()
        {
            return DataConfiguration.TeamsFileName
                .GetFilePath()
                .LoadFile()
                .ToTeamModels();
        }

        public List<TournamentModel> GetTournaments()
        {
            return DataConfiguration.TournamentsFileName
                .GetFilePath()
                .LoadFile()
                .ToTournamentModels();
        }

        // TODO: Add match update support
        public MatchModel UpdateMatch(MatchModel match)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}