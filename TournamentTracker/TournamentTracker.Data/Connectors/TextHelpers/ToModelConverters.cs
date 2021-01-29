using System.Collections.Generic;
using System.Linq;

using TournamentTracker.Domain.Models;
using static TournamentTracker.Data.Connectors.TextHelpers.DataLoaders;

namespace TournamentTracker.Data.Connectors.TextHelpers
{
    public static class ToModelConverters
    {
        public static List<PersonModel> ToPersonModels(this List<string> lines)
        {
            var output = new List<PersonModel>();

            foreach (var line in lines)
            {
                string[] cols = line.Split(',');

                var model = new PersonModel
                {
                    Id = int.Parse(cols[0]),
                    FirstName = cols[1],
                    LastName = cols[2],
                    EmailAddress = cols[3],
                    CellphoneNumber = cols[4]
                };

                output.Add(model);
            }

            return output;
        }

        public static List<PrizeModel> ToPrizeModels(this List<string> lines)
        {
            var output = new List<PrizeModel>();

            foreach (var line in lines)
            {
                string[] cols = line.Split(',');

                var model = new PrizeModel
                {
                    Id = int.Parse(cols[0]),
                    PlaceNumber = int.Parse(cols[1]),
                    PlaceName = cols[2],
                    PrizeName = cols[3],
                    PrizeAmount = decimal.Parse(cols[4]),
                    PrizePercentage = double.Parse(cols[5])
                };

                output.Add(model);
            }

            return output;
        }

        public static List<TeamModel> ToTeamModels(this List<string> lines)
        {
            var output = new List<TeamModel>();

            foreach (var line in lines)
            {
                string[] cols = line.Split(',');
                
                List<PersonModel> teamMembers = cols[2].Split('|').GetParticipantsByIds();

                var model = new TeamModel
                {
                    Id = int.Parse(cols[0]),
                    TeamName = cols[1],
                    TeamMembers = teamMembers
                };

                output.Add(model);
            }

            return output;
        }

        public static List<TournamentModel> ToTournamentModels(this List<string> lines)
        {
            var output = new List<TournamentModel>();

            foreach (var line in lines)
            {
                string[] cols = line.Split(',');

                List<TeamModel> teams = cols[3].Split('|').GetTeamsByIds();
                List<PrizeModel> prizes = cols[4].Split('|').GetPrizesByIds();
                List<List<MatchModel>> rounds = cols[5].Split('|').GetRounds();

                var model = new TournamentModel
                {
                    Id = int.Parse(cols[0]),
                    TournamentName = cols[1],
                    EntryFee = decimal.Parse(cols[2]),
                    EnteredTeams = teams,
                    Prizes = prizes,
                    Rounds = rounds
                };

                output.Add(model);
            }

            return output;
        }

        [System.Obsolete]
        private static List<MatchEntryModel> ToMatchEntryModel(string input)
        {
            var output = new List<MatchEntryModel>();
            List<MatchEntryModel> entries = DataConfiguration.MatchEntriesFileName
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
                string[] cols = line.Split(',');

                MatchEntryModel model = new MatchEntryModel
                {
                    Id = int.Parse(cols[0]),
                    Score = double.Parse(cols[2]),
                    CompetingTeam = null,
                    ParentMatch = null
                };

                if (int.TryParse(cols[1], out int teamId))
                {
                    model.CompetingTeam = GetTeamById(teamId);
                }
                
                if (int.TryParse(cols[3], out int id))
                {
                    model.ParentMatch = GetMatchById(id);
                }

                output.Add(model);
            }

            return output;
        }

        public static List<MatchModel> ToMatchModels(this List<string> lines)
        {
            List<MatchModel> output = new List<MatchModel>();

            foreach (var line in lines)
            {
                string[] cols = line.Split(',');

                var model = new MatchModel
                {
                    Id = int.Parse(cols[0]),
                    RoundNumber = int.Parse(cols[3]),
                    Winner = null
                };

                List<MatchEntryModel> entries = cols[1].Split('|').GetEntriesByIds();
                model.Entries.AddRange(entries);

                if (int.TryParse(cols[2], out int id))
                {
                    model.Winner = GetTeamById(id);
                }

                output.Add(model);
            }

            return output;
        }
    }
}
