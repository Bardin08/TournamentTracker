using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using TournamentTracker.Models;

namespace TournamentTracker.Logic
{
    public static class TournamentLogic
    {

        // REFACTOR: Move to a tournament model. GRASP patterns
        public static void CreateRounds(TournamentModel tournament)
        {
            List<TeamModel> shuffledTeams = ShuffleTeams(tournament.EnteredTeams);
            int rounds = FindNumberOfRounds(shuffledTeams.Count);
            int byes = FindNumberOfByes(shuffledTeams.Count, rounds);

            tournament.Rounds.Add(CreateFirstRound(byes, shuffledTeams));
            CreateOtherRounds(tournament, rounds);
        }

        private static void CreateOtherRounds(TournamentModel tournament, int rounds)
        {
            int round = 2;
            List<MatchModel> previousRound = tournament.Rounds[0];
            List<MatchModel> currentRound = new List<MatchModel>();
            MatchModel currentMatch = new MatchModel();

            while (round <= rounds)
            {
                foreach (var match in previousRound)
                {
                    currentMatch.Entries.Add(new MatchEntryModel { ParentMatch = match });

                    if (currentMatch.Entries.Count > 1)
                    {
                        currentMatch.RoundNumber = round;
                        currentRound.Add(currentMatch);
                        currentMatch = new MatchModel();
                    }
                }
                tournament.Rounds.Add(currentRound);
                previousRound = currentRound;

                currentRound = new List<MatchModel>();
                ++round;
            }
        }

        private static List<MatchModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchModel> output = new List<MatchModel>();
            MatchModel currentMatch = new MatchModel();

            foreach (var team in teams)
            {
                currentMatch.Entries.Add(new MatchEntryModel { CompetingTeam = team });

                if (byes > 0 || currentMatch.Entries.Count > 1)
                {
                    currentMatch.RoundNumber = 1;
                    output.Add(currentMatch);
                    currentMatch = new MatchModel();

                    if (byes > 0)
                    {
                        --byes; 
                    }
                }
            }

            return output;
        }

        private static int FindNumberOfByes(int teamsAmount, int rounds)
        {
            int totalTeams = 1;

            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            return totalTeams - teamsAmount;
        }

        private static int FindNumberOfRounds(int teamsAmount)
        {
            int output = 1, val = 2;
            
            while (val < teamsAmount)
            {
                ++output;
                val *= 2;
            }

            return output;
        }

        private static List<TeamModel> ShuffleTeams(List<TeamModel> teams)
        {
            return teams.OrderBy(x => Guid.NewGuid()).ToList();
        }

        public static void Notify(TournamentModel tournament, MatchModel match)
        {
            var sb = new StringBuilder();

            sb.Append($"Match between *{match.Entries[0].CompetingTeam.TeamName} *" +
                $"and *{match.Entries[1].CompetingTeam.TeamName}* ended.");
            
            sb.AppendLine();
            sb.AppendLine();

            sb.Append($"Final score: " +
                $"{match.Entries[0].Score}:{match.Entries[1].Score}.");
            
            sb.AppendLine();

            sb.Append($"The winner is: *{match.Winner.TeamName}*");
            foreach (var ns in GlobalConfiguration.NotificationSources)
            {
                ns.Notify(sb.ToString());
            }

            if (IsRoundEnded(tournament, match.RoundNumber))
            {
                sb.Clear();

                sb.Append($"Round {match.RoundNumber} ended.");

                foreach (var ns in GlobalConfiguration.NotificationSources)
                {
                    ns.Notify(sb.ToString());
                }
            }

            if (IsTournamentFinished(tournament, match))
            {
                sb.Clear();

                sb.Append($"{tournament.TournamentName} end, the winner is {match.Winner.TeamName}.");

                var prize = tournament.Prizes.FirstOrDefault(x => x.PlaceNumber == 1);

                if (prize != null)
                {
                    sb.Append($"They have won {prize.PrizeAmount}$");
                }
            }
        }

        private static bool IsTournamentFinished(TournamentModel tournament, MatchModel match)
        {
            if (match.RoundNumber == tournament.Rounds.Count && IsRoundEnded(tournament, match.RoundNumber))
            { 
                return true;
            }

            return false;
        }

        private static bool IsRoundEnded(TournamentModel tournament, int roundNumber)
        {
            foreach (var m in tournament.Rounds[roundNumber - 1])
            {
                if (m.Winner == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
