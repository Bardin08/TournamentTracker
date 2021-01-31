using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using TournamentTracker.Domain.Models;
using TournamentTracker.BusinessLogic;
using System.Text;
using System;

namespace TournamentTrackerWPFUI.ViewModels
{
    public class TournamentViewerViewModel : INotifyPropertyChanged
    {
        #region Events, Variables, Properties

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Variables

        private ObservableCollection<MatchModel> _matchesForCurrentRound;
        private ObservableCollection<MatchModel> _unplayedMatchesForCurrentRound;
        private ObservableCollection<MatchModel> _matchesToShow;

        private MatchModel _selectedMatch;

        private bool _showOnlyUnplayed;

        private int _currentRound;

        #endregion

        #region Properties

        public TournamentModel Tournament { get; set; }

        public List<int> Rounds { get; set; }

        public ObservableCollection<MatchModel> MatchesForCurrentRound
        {
            get => _matchesForCurrentRound;
            set
            {
                _matchesForCurrentRound = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MatchesForCurrentRound)));
            }
        }

        public ObservableCollection<MatchModel> UnplayedMatchesForCurrentRound
        {
            get => _unplayedMatchesForCurrentRound;
            set
            {
                _unplayedMatchesForCurrentRound = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnplayedMatchesForCurrentRound)));
            }
        }

        public ObservableCollection<MatchModel> MatchesToShow
        {
            get => _matchesToShow;
            set
            {
                _matchesToShow = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MatchesToShow)));
            }
        }

        public MatchModel SelectedMatch
        {
            get => _selectedMatch;
            set
            {
                _selectedMatch = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMatch)));
            }
        }

        public bool ShowOnlyUnplayed
        {
            get => _showOnlyUnplayed;
            set
            {
                _showOnlyUnplayed = value;

                if (_showOnlyUnplayed)
                {
                    MatchesToShow = UnplayedMatchesForCurrentRound;
                }
                else
                {
                    MatchesToShow = MatchesForCurrentRound;
                }
            }
        }
            
        public int CurrentRound
        {
            get => _currentRound;
            set
            {
                _currentRound = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentRound)));
                UpdateMatches();
            }
        }

        #endregion

        #endregion

        #region Methods

        public TournamentViewerViewModel(TournamentModel tournament)
        {
            Tournament = tournament;

            PrepareDataForView();
        }

        public void SelectMatch(MatchModel match)
        {
            SelectedMatch = match;
        }
        
        public void Notify()
        {
            foreach (var ns in GlobalConfiguration.NotificationSources)
            {
                ns.Notify(() =>
                {
                    var sb = new StringBuilder();

                    sb.Append($"Tournament: {Tournament.TournamentName}\n\n");

                    sb.AppendLine();

                    sb.Append($"Match between *{SelectedMatch.Entries[0].CompetingTeam.TeamName}*" +
                        $" and *{SelectedMatch.Entries[1].CompetingTeam.TeamName}* ended.");

                    sb.AppendLine();
                    sb.AppendLine();

                    sb.Append($"Final score: " +
                        $"{SelectedMatch.Entries[0].Score}:{SelectedMatch.Entries[1].Score}.");

                    sb.AppendLine();

                    sb.Append($"The winner is: *{SelectedMatch.Winner.TeamName}*");

                    return sb.ToString();
                });
            }
        }

        private bool IsRoundEnd()
        {
            foreach (var m in Tournament.Rounds[CurrentRound - 1])
            {
                if (m.Winner == null)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsTournamentEnd()
        {
            if (CurrentRound != Tournament.Rounds.Count)
            {
                return false;
            }

            return IsRoundEnd();
        }

        public void WriteMatchResult(int firstTeamScore, int secondTeamScore)
        {
            int winnerIndex = 0;
            if (secondTeamScore > firstTeamScore)
            {
                winnerIndex = 1;
            }

            SelectedMatch.Winner = SelectedMatch.Entries[winnerIndex].CompetingTeam;

            SelectedMatch.Entries[0].Score = firstTeamScore;
            SelectedMatch.Entries[1].Score = secondTeamScore;

            if (CurrentRound < Rounds.Last())
            { 
                MoveTeamToNextRound(SelectedMatch);
            }

            GlobalConfiguration.Connection.UpdateMatch(SelectedMatch);

            SendNotification();

            UnplayedMatchesForCurrentRound.Remove(SelectedMatch);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(UnplayedMatchesForCurrentRound)));
        }

        private void SendNotification() 
        {
            foreach (var ns in GlobalConfiguration.NotificationSources)
            {
                ns.Notify(() =>
                {
                    var sb = new StringBuilder();

                    sb.Append($"Tournament: {Tournament.TournamentName}\n\n");

                    sb.AppendLine();

                    sb.Append($"Match between *{SelectedMatch.Entries[0].CompetingTeam.TeamName}*" +
                        $"and *{SelectedMatch.Entries[1].CompetingTeam.TeamName}* ended.");

                    sb.AppendLine();
                    sb.AppendLine();

                    sb.Append($"Final score: " +
                        $"{SelectedMatch.Entries[0].Score}:{SelectedMatch.Entries[1].Score}.");

                    sb.AppendLine();

                    sb.Append($"The winner is: *{SelectedMatch.Winner.TeamName}*");

                    return sb.ToString();
                });
            }
        }

        private void UpdateMatches()
        {
            if (CurrentRound >= 1)
            {
                MatchesForCurrentRound =
                    new ObservableCollection<MatchModel>(Tournament.Rounds[CurrentRound - 1]);
            }
            else
            {
                MatchesForCurrentRound = new ObservableCollection<MatchModel>();
            }
            
            UnplayedMatchesForCurrentRound =
                new ObservableCollection<MatchModel>(MatchesForCurrentRound.Where(m => m.Winner == null));

            MatchesToShow = MatchesForCurrentRound;
                
            if (ShowOnlyUnplayed)
            {
                MatchesToShow = UnplayedMatchesForCurrentRound;
            }    
        }

        private void PrepareDataForView()
        {
            Rounds = new List<int>();

            for (int i = 1; i <= Tournament.Rounds.Count; ++i)
            {
                Rounds.Add(i);
            }

            CurrentRound = 1;

            MatchesForCurrentRound =
                new ObservableCollection<MatchModel>(Tournament.Rounds[CurrentRound - 1]);

            FinishMatchesWithByes();

            MatchesToShow = MatchesForCurrentRound;

            UnplayedMatchesForCurrentRound = 
                new ObservableCollection<MatchModel>(MatchesForCurrentRound.Where(m => m.Winner == null));

            SelectedMatch = MatchesToShow[0];
        }

        private void FinishMatchesWithByes()
        {
            foreach (var match in MatchesForCurrentRound.Where(m => m.Entries.Count == 1))
            {
                match.Entries[0].Score = 3;
                match.Winner = match.Entries[0].CompetingTeam;

                match.Entries.Add(new MatchEntryModel
                {
                    CompetingTeam = new TeamModel { TeamName = "Bye" }
                });

                if (CurrentRound < Rounds.Last())
                {
                    MoveTeamToNextRound(match);
                }

                GlobalConfiguration.Connection.UpdateMatch(match);
            }
        }

        private void MoveTeamToNextRound(MatchModel match)
        {
            foreach (var m in Tournament.Rounds[CurrentRound])
            {
                // r -- Free slot for team at the next round
                var r = m.Entries.FirstOrDefault(me => me.CompetingTeam == null);

                if (r != null)
                {
                    r.ParentMatch = match;
                    r.CompetingTeam = match.Winner;

                    break;
                }
            }
        }

        #endregion
    }
}
