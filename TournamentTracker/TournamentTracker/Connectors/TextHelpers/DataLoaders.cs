using System;
using System.Linq;
using System.Collections.Generic;

using TournamentTracker.Models;

namespace TournamentTracker.Connectors.TextHelpers
{
    public static class DataLoaders
    {
        public static List<PersonModel> GetParticipantsByIds(this string[] ids)
        {
            var participants = GlobalConfiguration.PeopleFileName
                .GetFilePath()
                .LoadFile()
                .ToPersonModels();

            if (participants.Count < 1)
            {
                throw new Exception("Participants were not loaded!");
            }

            var output = new List<PersonModel>();

            foreach (var id in ids)
            {
                output.Add(participants.First(p => p.Id == int.Parse(id)));
            }

            return output;
        }

        public static TeamModel GetTeamById(int teamId) 
        {
            var teams = GlobalConfiguration.TeamsFileName
                .GetFilePath()
                .LoadFile()
                .ToTeamModels();

            return teams.First(x => x.Id == teamId) ?? throw new Exception("Team with id " + teamId + " not found.");
        }

        public static List<TeamModel> GetTeamsByIds(this string[] ids)
        {
            var allTeams = GlobalConfiguration.TeamsFileName
                .GetFilePath()
                .LoadFile()
                .ToTeamModels();

            if (allTeams.Count < 1)
            {
                throw new Exception("Teams were not loaded!");
            }

            var output = new List<TeamModel>();

            foreach (var teamId in ids)
            {
                output.Add(allTeams.First(p => p.Id == int.Parse(teamId)));
            }

            return output;
        }

        public static List<PrizeModel> GetPrizesByIds(this string[] ids)
        {
            var allPrizes = GlobalConfiguration.PrizesFileName
                .GetFilePath()
                .LoadFile()
                .ToPrizeModels();

            var output = new List<PrizeModel>();

            if (allPrizes.Count > 0)
            {
                foreach (var id in ids)
                {
                    output.Add(allPrizes.First(p => p.Id == int.Parse(id)));
                }
            }

            return output;
        }

        public static MatchModel GetMatchById(int matchId)
        {
            var matches = GlobalConfiguration.MatchesFileName
                .GetFilePath()
                .LoadFile()
                .ToMatchModels();

            return matches.First(x => x.Id == matchId);
        }

        public static List<List<MatchModel>> GetRounds(this string[] ids)
        {
            var matches = GlobalConfiguration.MatchesFileName
                .GetFilePath()
                .LoadFile()
                .ToMatchModels();

            var output = new List<List<MatchModel>>();

            var ms = new List<MatchModel>();
            foreach (var round in ids)
            {
                var msText = round.Split('^');

                foreach (var matchModelTextId in msText)
                {
                    ms.Add(matches.First(x => x.Id == int.Parse(matchModelTextId)));
                }

                output.Add(ms);
            }

            return output;
        }
    
        public static List<MatchEntryModel> GetEntriesByIds(this string[] ids)
        {
            var entries = GlobalConfiguration.MatchEntriesFileName
                .GetFilePath()
                .LoadFile()
                .ToMatchEntryModels();

            if (entries.Count < 1)
            {
                throw new Exception("Entries were not loaded!");
            }

            var output = new List<MatchEntryModel>();

            foreach (var id in ids)
            {
                output.Add(entries.First(e => e.Id == int.Parse(id)));
            }

            return output;
        }
    }
}
