using TournamentTracker.Models;
using System.Collections.Generic;

namespace TournamentTracker.Connectors.TextHelpers
{
    public static class ToStringConverters
    { 
        public static List<string> PeopleToLines(this List<PersonModel> people)
        {
            var lines = new List<string>();

            foreach (var person in people)
            {
                lines.Add($"{person.Id},{person.FirstName},{person.LastName},{person.EmailAddress},{person.CellphoneNumber}");
            }

            return lines ?? new List<string>();
        }

        public static List<string> PrizesToLines(this List<PrizeModel> prizes)
        {
            var lines = new List<string>();

            foreach (var prize in prizes)
            {
                lines.Add($"{prize.Id},{prize.PlaceNumber},{prize.PlaceName},{prize.PrizeName},{prize.PrizeAmount},{prize.PrizePercentage}");
            }

            return lines ?? new List<string>();
        }

        public static List<string> TeamsToLines(this List<TeamModel> teams)
        {
            var lines = new List<string>();

            foreach (var team in teams)
            {
                var teamMembersIds = "";

                foreach (var teamMember in team.TeamMembers)
                {
                    teamMembersIds += $"{teamMember.Id}|";
                }

                lines.Add($"{team.Id},{team.TeamName},{teamMembersIds.TrimEnd('|')}");
            }

            return lines ?? new List<string>();
        }

        public static List<string> TournamentToLines(this List<TournamentModel> tournaments)
        {
            var lines = new List<string>();
            foreach (var t in tournaments)
            {
                // TODO: Move ids generate to a separate method which will generate ids to each type
                t.EnteredTeams.TeamsToLines().SaveToFile(GlobalConfiguration.TeamsFileName);
                t.Prizes.PrizesToLines().SaveToFile(GlobalConfiguration.PrizesFileName);

                var teamsIds = "";
                foreach (var team in t.EnteredTeams)
                { 
                    teamsIds += $"{team.Id}|";
                }

                var prizesIds = "";
                t.Prizes.ForEach(prize => prizesIds += $"{prize.Id}|");

                foreach (var prize in t.Prizes)
                { 
                    prizesIds += $"{prize.Id}|";
                }

                lines.Add($"{t.Id},{t.TournamentName},{t.EntryFee},{teamsIds.TrimEnd('|')},{prizesIds.TrimEnd('|')},{t.Rounds.RoundsToString()}");
            }

            return lines ?? new List<string>();
        }

        public static List<string> MatchesToLines(this List<MatchModel> matches)
        {
            List<string> lines = new List<string>();

            foreach (var m in matches)
            {
                string winner = "";
                if (m.Winner != null)
                {
                    winner = m.Winner.Id.ToString();
                }

                lines.Add($"{m.Id},{MatchEntriesToString(m.Entries)},{winner},{m.RoundNumber}");
            }

            return lines ?? new List<string>();
        }
        
        public static List<string> MatchEntriesToLines(this List<MatchEntryModel> entries)
        {
            List<string> lines = new List<string>();

            foreach (var e in entries)
            {
                string parent = "";
                if (e.ParentMatch != null)
                {
                    parent = e.ParentMatch.Id.ToString();
                }

                string competingTeam = "";
                if (e.CompetingTeam != null)
                {
                    competingTeam = e.CompetingTeam.Id.ToString();
                }

                lines.Add($"{e.Id},{competingTeam},{e.Score},{parent}");
            }

            return lines;
        }

        public static string MatchEntriesToString(this List<MatchEntryModel> matchEntries)
        {
            var entriesIds = "";

            foreach (var e in matchEntries)
            {
                entriesIds += $"{e.Id}|";
            }

            return entriesIds.TrimEnd('|');
        }

        public static string RoundsToString(this List<List<MatchModel>> rounds)
        {
            string output = "";

            foreach (var round in rounds)
            { 
                foreach (var match in round)
                {
                    output += $"{match.Id}^";
                }

                output = output.TrimEnd('^');
                output += "|";
            }


            return output.TrimEnd('|');
        }

    }
}
