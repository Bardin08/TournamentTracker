using System;
using System.Collections.Generic;
using System.Linq;

namespace TournamentTracker.Domain.Models
{
    public class TournamentModel
    {
        /// <summary>
        /// Represents tournament unique identifier.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Represents the tournament name.
        /// </summary>
        public string TournamentName { get; set; }
        
        /// <summary>
        /// Represents the tournament entry fee.
        /// </summary>
        public decimal EntryFee { get; set; }
        
        /// <summary>
        /// Represents the tournament teams.
        /// </summary>
        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();
        
        /// <summary>
        /// Represents the tournament prizes.
        /// </summary>
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();
        
        /// <summary>
        /// Represents the tournament rounds.
        /// </summary>
        public List<List<MatchModel>> Rounds { get; set; } = new List<List<MatchModel>>();
    

        /// <summary>
        /// Create rounds for the current tournament model.
        /// </summary>
        public void CreateRounds()
        {
            EnteredTeams = ShuffleTeams(EnteredTeams);
            int rounds = FindNumberOfRounds();
            int byes = FindNumberOfByes();

            Rounds.Add(CreateFirstRound(byes));
            CreateOtherRounds(rounds);
        }

        /// <summary>
        /// Create matches for the first round.
        /// </summary>
        /// <param name="byes"> Number of byes. </param>
        /// <returns></returns>
        private List<MatchModel> CreateFirstRound(int byes)
        {
            List<MatchModel> output = new List<MatchModel>();
            MatchModel currentMatch = new MatchModel();

            foreach (var team in EnteredTeams)
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

        /// <summary>
        /// Create matches for all rounds except first.
        /// </summary>
        /// <param name="rounds"> Rounds amount. </param>
        private void CreateOtherRounds(int rounds)
        {
            int round = 2;
            List<MatchModel> previousRound = Rounds[0];
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
                Rounds.Add(currentRound);
                previousRound = currentRound;

                currentRound = new List<MatchModel>();
                ++round;
            }
        }

        /// <summary>
        /// Returns the number of byes that should be added.
        /// </summary>
        /// <returns></returns>
        private int FindNumberOfByes()
        {
            int totalTeams = 1;

            for (int i = 1; i <= FindNumberOfRounds(); i++)
            {
                totalTeams *= 2;
            }

            return totalTeams - EnteredTeams.Count;
        }

        /// <summary>
        /// Return the number of matches which will be played.
        /// </summary>
        private int FindNumberOfRounds()
        {
            int output = 1, val = 2;

            while (val < EnteredTeams.Count)
            {
                ++output;
                val *= 2;
            }

            return output;
        }

        /// <summary>
        /// Shuffle teams and return a shuffled list.
        /// </summary>
        /// <param name="teams"> The list of teams. </param>
        private List<TeamModel> ShuffleTeams(List<TeamModel> teams)
        {
            return teams.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}
