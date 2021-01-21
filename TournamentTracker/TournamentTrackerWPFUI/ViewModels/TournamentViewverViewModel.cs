using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using TournamentTracker.Models;

namespace TournamentTrackerWPFUI.ViewModels
{
    public class TournamentViewverViewModel : INotifyPropertyChanged, INotifyCollectionChanged
    {
        #region Events, Properties and Variables

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private int _currentRound;
        private MatchModel _selectedMatch;
        private TeamModel _team1;
        private TeamModel _team2;
        private ObservableCollection<MatchModel> _unplayedMatchesForCurrentRound;
        private ObservableCollection<MatchModel> _matchesForCurrentRound;
        private ObservableCollection<MatchModel> _matchesToShow;

        public int CurrentRound 
        {
            get => _currentRound;
            set 
            {
                _currentRound = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentRound)));
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

        public TeamModel Team1 
        {
            get => _team1;
            set
            {
                _team1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Team1)));
            }
        }

        public TeamModel Team2
        {
            get => _team2;
            set
            {
                _team2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Team2)));
            }
        }

        public TournamentModel Tournament { get; private set; }

        public List<int> Rounds { get; }

        public ObservableCollection<MatchModel> MatchesToShow
        {
            get => _matchesToShow;
            set
            {
                _matchesToShow = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public ObservableCollection<MatchModel> UnplayedMatchesForCurrentRound
        {
            get => _unplayedMatchesForCurrentRound;
            set 
            {
                _unplayedMatchesForCurrentRound = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
        
        public ObservableCollection<MatchModel> AllMatchesForCurrentRound 
        {
            get => _matchesForCurrentRound;
            set
            {
                _matchesForCurrentRound = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        #endregion

        public TournamentViewverViewModel(TournamentModel tournament)
        {
            Tournament = tournament;

            Rounds = new List<int>();
            for (int i = 1; i <= tournament.Rounds.Count; ++i)
            {
                Rounds.Add(i);
            }

            CurrentRound = 1;

            AllMatchesForCurrentRound = new ObservableCollection<MatchModel>(
                tournament.Rounds[CurrentRound - 1]);
            
            CompleteMatchesWithByes();
            
            if (AllMatchesForCurrentRound.Count > 0)
            {
                SelectMatch(AllMatchesForCurrentRound[0]);
            }

            UnplayedMatchesForCurrentRound = new ObservableCollection<MatchModel>(
                AllMatchesForCurrentRound.Where(m => m.Winner == null).ToList());

            MatchesToShow = AllMatchesForCurrentRound;
        }

        public void SelectMatch(MatchModel match)
        {
            Team1 = match.Entries[0].CompetingTeam;

            if (match.Entries.Count > 1)
            {
                Team2 = match.Entries[1]?.CompetingTeam;
            }
            else
            {
                Team2 = new TeamModel
                {
                    TeamName = "Bye"
                };
            }
        }

        public void ShowOnlyUnplayed(bool isChecked)
        {
            if (isChecked)
            {
                MatchesToShow = UnplayedMatchesForCurrentRound;
            }
            else
            {
                MatchesToShow = AllMatchesForCurrentRound;
            }
        }

        public void CompleteMatchesWithByes()
        {
            foreach (var match in AllMatchesForCurrentRound)
            { 
                if (match.Entries.Count == 1)
                {
                    match.Winner = match.Entries[0].CompetingTeam;
                }
            }
        }
    }
}
