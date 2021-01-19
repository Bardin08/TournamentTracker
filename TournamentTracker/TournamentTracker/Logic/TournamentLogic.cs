using System;
using System.Collections.Generic;
using System.Linq;
using TournamentTracker.Models;

namespace TournamentTracker.Logic
{
    public static class TournamentLogic
    {
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
    }
}
