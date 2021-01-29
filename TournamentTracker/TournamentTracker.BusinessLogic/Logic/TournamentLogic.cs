using System.Linq;
using System.Text;

using TournamentTracker.Domain.Models;

namespace TournamentTracker.BusinessLogic.Logic
{
    public static class TournamentLogic
    {
        // REFACTOR: Move to notifier
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
