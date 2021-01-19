using System;
using System.IO;
using System.Linq;
using System.Configuration;
using TournamentTracker.Models;
using System.Collections.Generic;

namespace TournamentTracker.Connectors.TextHelpers
{
    public static class CSVFileProcessor
    {
        public static string GetFilePath(this string fileName) 
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                ConfigurationManager.AppSettings["DataFilesDirectory"];
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory + $"\\{fileName}";
        }

        public static List<string> LoadFile(this string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<string>();
            }

            return File.ReadAllLines(filePath).ToList();
        }

        public static void SaveToPrizesFile(this List<PrizeModel> prizes)
        {
            var lines = new List<string>();

            foreach (var prize in prizes)
            {
                lines.Add($"{prize.Id},{prize.PlaceNumber},{prize.PlaceName},{prize.PrizeName},{prize.PrizeAmount},{prize.PrizePercentage}");
            }

            File.WriteAllLinesAsync(GlobalConfiguration.PrizesFileName.GetFilePath(), lines);
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

        public static void SaveToPeopleFile(this List<PersonModel> people)
        {
            var lines = new List<string>();

            foreach (var person in people)
            {
                lines.Add($"{person.Id},{person.FirstName},{person.LastName},{person.EmailAddress},{person.CellphoneNumber}");
            }

            File.WriteAllLines(GlobalConfiguration.PeopleFileName.GetFilePath(), lines);
        }

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

        public static void SaveToTeamsFile(this List<TeamModel> teamModels)
        {
            var teams = new List<string>();

            foreach (var team in teamModels)
            {
                var teamMembersIds = "";

                foreach (var teamMember in team.TeamMembers)
                {
                    teamMembersIds += $"{teamMember.Id}|";
                }

                teams.Add($"{team.Id},{team.TeamName},{teamMembersIds.TrimEnd('|')}");
            }

            File.WriteAllLines(GlobalConfiguration.TeamsFileName.GetFilePath(), teams);
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
            /**
             * Tournament record pattern: 
             *         id,
             *         tournament name,
             *         entry fee,
             *         teams ids(separated with pipes "|"),
             *         prizes ids(separated with pipes "|"),
             *         matches(rounds separated with pipes "|" and matches with ";" two teams who`re playing separated with "^")
             *         
             * Tournament record example:
             *         1,Tournament name,100,1|2|3|4,1|2,2^3;1^4|1^3
             *         
             **/
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

            foreach (var line in lines)
            {
                var cols = line.Split(',');

                while (cols.Length < 6)
                {
                    cols.Append("");
                }

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
                var ms = new List<MatchModel>();

                var model = new TournamentModel
                {
                    Id = int.Parse(cols[0]),
                    TournamentName = cols[1],
                    EntryFee = decimal.Parse(cols[2]),
                    EnteredTeams = tournamentTeams,
                    Prizes = tournamentPrizes,
                };

                foreach (var round in rounds)
                {
                    var msText = round.Split('^');

                    foreach (var matchModelTextId in msText)
                    {
                        ms.Add(matches.Where(x => x.Id == int.Parse(matchModelTextId)).First());   
                    }

                    model.Rounds.Add(ms);
                }

                output.Add(model);
            }

            return output;
        }

        public static void SaveToTournamentsFile(this List<TournamentModel> tournaments)
        {
            var lines = new List<string>();
            foreach (var t in tournaments)
            {
                t.EnteredTeams.SaveToTeamsFile();
                t.Prizes.SaveToPrizesFile();

                var teamsIds = "";
                t.EnteredTeams.ForEach(team => teamsIds += $"{team.Id}|");

                var prizesIds = "";
                t.Prizes.ForEach(prize => prizesIds += $"{prize.Id}|");

                lines.Add($"{t.Id},{t.TournamentName},{t.EntryFee},{teamsIds.TrimEnd('|')},{prizesIds.TrimEnd('|')}");
            }

            File.WriteAllLines(GlobalConfiguration.TournamentsFileName.GetFilePath(), lines);
        }
    
        public static void SaveRoundsToFile(this TournamentModel tournament)
        {
            foreach (var round in tournament.Rounds)
            {
                foreach (var match in round)
                {
                    match.SaveMatchToFile();
                }
            }
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

        public static TeamModel GetTeamById(int teamId)
        {
            var teams = GlobalConfiguration.TeamsFileName
                .GetFilePath()
                .LoadFile()
                .ToTeamModels();

            return teams.First(x => x.Id == teamId) ?? throw new Exception("Team with id " + teamId + " not found.");
        }

        public static List<MatchEntryModel> ToMatchEntryModels(this List<string> lines)
        {
            var output = new List<MatchEntryModel>();

            foreach (var line in lines)
            {
                var cols = line.Split(',');

                MatchEntryModel model = new MatchEntryModel();

                model.Id = int.Parse(cols[0]);
                model.CompetingTeam = GetTeamById(int.Parse(cols[1]));
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

        public static MatchModel GetMatchById(int matchId)
        {
            var matches = GlobalConfiguration.MatchesFileName
                .GetFilePath()
                .LoadFile()
                .ToMatchModels();

            return matches.First(x => x.Id == matchId);
        }

        public static List<MatchModel> ToMatchModels(this List<string> lines)
        {
            List<MatchModel> output = new List<MatchModel>();
            
            foreach (var line in lines)
            {
                string[] cols = line.Split(',');

                var model = new MatchModel();
                model.Id = int.Parse(cols[0]);
                model.Entries = null;

                if (int.TryParse(cols[2], out int id))
                {
                    model.Winner = GetTeamById(id);
                }
                else
                {
                    model.Winner = null;
                }
                model.RoundNumber = int.Parse(cols[3]);
                
                output.Add(model);
            }

            return output;
        }

        public static void SaveMatchToFile(this MatchModel match)
        {
            List<MatchModel> matches = GlobalConfiguration.MatchesFileName
                .GetFilePath()
                .LoadFile()
                .ToMatchModels();

            int lastSavedId = 0;
            if (matches.Count > 0)
            {
                lastSavedId = matches.OrderByDescending(x => x.Id).First().Id;
            }
            match.Id = lastSavedId + 1;

            foreach (var entry in match.Entries)
            {
                entry.SaveMatchEntryToFile();
            }

            List<string> lines = new List<string>();

            matches.Add(match);

            foreach (var m in matches)
            {
                string winner = "";
                if (m.Winner != null)
                {
                    winner = m.Winner.Id.ToString();
                }

                lines.Add($"{m.Id},{ConvertMatchEntriesToString(m.Entries)},{winner},{m.RoundNumber}");
            }

            File.WriteAllLines(GlobalConfiguration.MatchesFileName.GetFilePath(), lines);
        }

        private static string ConvertMatchEntriesToString(List<MatchEntryModel> matchEntries)
        {
            var entriesIds = "";

            foreach (var e in matchEntries)
            {
                entriesIds += $"{e.Id}|";
            }

            return entriesIds.TrimEnd('|');
        }

        public static void SaveMatchEntryToFile(this MatchEntryModel entry)
        {
            List<MatchEntryModel> entries = GlobalConfiguration.MatchEntriesFileName
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


            File.WriteAllLines(GlobalConfiguration.MatchEntriesFileName.GetFilePath(), MatchEntriesToLines(entries));
        }

        private static List<string> MatchEntriesToLines(List<MatchEntryModel> entries)
        {
            List<string> lines = new List<string>();

            foreach (var e in entries)
            {
                string parent = "";
                if (e.ParentMatch != null)
                {
                    parent = e.ParentMatch.Id.ToString();
                }

                lines.Add($"{e.Id},{e.CompetingTeam.Id},{e.Score},{parent}");
            }

            return lines;
        }
    }
}
