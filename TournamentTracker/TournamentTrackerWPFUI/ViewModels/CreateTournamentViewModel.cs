using System;
using TournamentTracker.Models;
using TournamentTracker.Interfaces;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace TournamentTrackerWPFUI.ViewModels
{
    public class CreateTournamentViewModel : INotifyCollectionChanged
    {
        #region Variables, Events and Properties

        private ObservableCollection<TeamModel> _teams;
        private ObservableCollection<PrizeModel> _prizes;
        private ObservableCollection<TeamModel> _selectedTeams;
        private ObservableCollection<PrizeModel> _selectedPrizes;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public ObservableCollection<TeamModel> Teams
        {
            get { return _teams; }
            set 
            {
                _teams = value ?? throw new ArgumentNullException(nameof(value));
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public ObservableCollection<PrizeModel> Prizes
        {
            get { return _prizes; }
            set {
                _prizes = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public ObservableCollection<TeamModel> SelectedTeams
        {
            get { return _selectedTeams; }
            set 
            {
                _selectedTeams = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public ObservableCollection<PrizeModel> SelectedPrizes
        {
            get { return _selectedPrizes; }
            set 
            {
                _selectedPrizes = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        #endregion

        public CreateTournamentViewModel(IDataConnection dataSource)
        {
            Teams = new ObservableCollection<TeamModel>(dataSource.GetTeams());
            Prizes = new ObservableCollection<PrizeModel>(dataSource.GetPrizes());

            SelectedTeams = new ObservableCollection<TeamModel>();
            SelectedPrizes = new ObservableCollection<PrizeModel>();
        }

        public void CreateTeam(TeamModel team)
        {
            Teams.Add(team);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
        }
        
        public void CreatePrize(PrizeModel prize)
        {
            Prizes.Add(prize);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
        }

        public void AddTeamToTournament(TeamModel team)
        {
            Teams.Remove(team);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
            SelectedTeams.Add(team);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
        }

        public void AddPrizeToTournament(PrizeModel prize)
        {
            Prizes.Remove(prize);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
            SelectedPrizes.Add(prize);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
        }
    }
}
