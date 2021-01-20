using System;
using System.Collections.Generic;
using System.Linq;
using TournamentTracker.Models;

namespace TournamentTracker.Connectors.TextHelpers
{
    public static class ToModelConverters
    {
        public static List<PersonModel> ToPersonModels(this List<string> lines)
        {
            var output = new List<PersonModel>();

            foreach (var line in lines)
            {
                var cols = line.Split(',');

                output.Add(new PersonModel
                {
                    Id = int.Parse(cols[0]),
                    FirstName = cols[1],
                    LastName = cols[2],
                    EmailAddress = cols[3],
                    CellphoneNumber = cols[4]
                });
            }

            return output;
        }

        public static List<PrizeModel> ToPrizeModels(this List<string> lines)
        {
            var output = new List<PrizeModel>();

            foreach (var line in lines)
            {
                var cols = line.Split(',');

                output.Add(new PrizeModel()
                {
                    Id = int.Parse(cols[0]),
                    PlaceNumber = int.Parse(cols[1]),
                    PlaceName = cols[2],
                    PrizeName = cols[3],
                    PrizeAmount = decimal.Parse(cols[4]),
                    PrizePercentage = double.Parse(cols[5])
                });
            }

            return output;
        }

        // BUG: Check if team members are load
        public static List<TeamModel> ToTeamModels(this List<string> lines)
        {
            var output = new List<TeamModel>();
            var participants = GlobalConfiguration.PeopleFileName
                .GetFilePath()
                .LoadFile()
                .ToPersonModels();

            foreach (var line in lines)
            {
                var cols = line.Split(',');
                var teamMembersIds = cols[2].Split('|');

                List<PersonModel> teamMembers = new List<PersonModel>();
                teamMembersIds.ToList()
                    .ForEach(x => teamMembers.Add(participants.First(p => p.Id == int.Parse(x))));

                output.Add(new TeamModel
                {
                    Id = int.Parse(cols[0]),
                    TeamName = cols[1],
                    TeamMembers = teamMembers
                });
            }

            return output;
        }

        public static List<TournamentModel> ToTournamentModels(this List<string> lines)
        {
            #region Load previous records from files

            var output = new List<TournamentModel>();
            var allTeams = GlobalConfiguration.TeamsFileName
                .GetFilePath()
                .LoadFile()
                .ToTeamModels();
            var allPrizes = GlobalConfiguration.PrizesFileName
                .GetFilePath()
                .LoadFile()
                .ToPrizeModels();
            var matches = GlobalConfiguration.MatchesFileName
                .GetFilePath()
                .LoadFile()
                .ToMatchModels();
            var matchEntries = GlobalConfiguration.MatchEntriesFileName
                .GetFilePath()
                .LoadFile()
                .ToMatchEntryModels();

            #endregion

            foreach (var line in lines)
            {
                var cols = line.Split(',');

                var teamsIds = cols[3].Split('|');
                var tournamentTeams = new List<TeamModel>();
                teamsIds.ToList().ForEach(id => tournamentTeams.Add(allTeams.First(p => p.Id == int.Parse(id))));

                var prizesIds = cols[4].Split('|');
                var tournamentPrizes = new List<PrizeModel>();

                if (allPrizes.Count > 0)
                {
                    prizesIds.ToList().ForEach(id => tournamentPrizes.Add(allPrizes.First(p => p.Id == int.Parse(id))));
                }
                else
                {
                    tournamentPrizes = new List<PrizeModel>();
                }

                var rounds = cols[5].Split('|');

                var model = new TournamentModel
                {
                    Id = int.Parse(cols[0]),
                    TournamentName = cols[1],
                    EntryFee = decimal.Parse(cols[2]),
                    EnteredTeams = tournamentTeams,
                    Prizes = tournamentPrizes,
                };

                var ms = new List<MatchModel>();
                foreach (var round in rounds)
                {
                    var msText = round.Split('^');

                    foreach (var matchModelTextId in msText)
                    {
                        ms.Add(matches.First(x => x.Id == int.Parse(matchModelTextId)));
                    }

                    model.Rounds.Add(ms);
                }

                output.Add(model);
            }

            return output;
        }

        private static List<MatchEntryModel> ToMatchEntryModels(string input)
        {
            var output = new List<MatchEntryModel>();
            List<MatchEntryModel> entries = GlobalConfiguration.MatchEntriesFileName
                .GetFilePath()
                .LoadFile()
                .ToMatchEntryModels();

            var ids = input.Split("|");

            foreach (var id in ids)
            {
                output.Add(entries.Where(x => x.Id == int.Parse(id)).First());
            }

            return output;
        }

        public static List<MatchEntryModel> ToMatchEntryModels(this List<string> lines)
        {
            var output = new List<MatchEntryModel>();

            foreach (var line in lines)
            {
                var cols = line.Split(',');

                MatchEntryModel model = new MatchEntryModel();

                model.Id = int.Parse(cols[0]);

                model.CompetingTeam = null;
                if (int.TryParse(cols[1], out int teamId))
                {
                    model.CompetingTeam = GetTeamById(teamId);
                }
                
                model.Score = double.Parse(cols[2]);

                if (int.TryParse(cols[3], out int id))
                {
                    model.ParentMatch = GetMatchById(id);
                }
                else
                {
                    model.ParentMatch = null;
                }

                output.Add(model);
            }

            return output;
        }

        public static List<MatchModel> ToMatchModels(this List<string> lines)
        {
            List<MatchModel> output = new List<MatchModel>();
            var entries = GlobalConfiguration.MatchEntriesFileName
                .GetFilePath()
                .LoadFile()
                .ToMatchEntryModels();

            foreach (var line in lines)
            {
                string[] cols = line.Split(',');

                var model = new MatchModel
                {
                    Id = int.Parse(cols[0]),
                    RoundNumber = int.Parse(cols[3])
                };

                var entriesIds = cols[1].Split('|');
                foreach (var entryId in entriesIds)
                {
                    model.Entries.Add(entries
                        .Where(e => e.Id == int.Parse(entryId))
                        .First());
                }

                if (int.TryParse(cols[2], out int id))
                {
                    model.Winner = GetTeamById(id);
                }
                else
                {
                    model.Winner = null;
                }

                output.Add(model);
            }

            return output;
        }

        private static TeamModel GetTeamById(int teamId)
        {
            var teams = GlobalConfiguration.TeamsFileName
                .GetFilePath()
                .LoadFile()
                .ToTeamModels();

            // TODO: Change to custom Exception
            return teams.First(x => x.Id == teamId) ?? throw new Exception("Team with id " + teamId + " not found.");
        }

        private static MatchModel GetMatchById(int matchId)
        {
            var matches = GlobalConfiguration.MatchesFileName
                .GetFilePath()
                .LoadFile()
                .ToMatchModels();

            return matches.First(x => x.Id == matchId);
        }
    }
}
