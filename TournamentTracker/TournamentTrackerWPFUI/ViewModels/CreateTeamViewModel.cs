using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using TournamentTracker.Models;

namespace TournamentTrackerWPFUI.ViewModels
{
    public class CreateTeamViewModel : INotifyPropertyChanged, INotifyCollectionChanged
    {
        #region Variables

        private ObservableCollection<PersonModel> _availableMembers;
        private ObservableCollection<PersonModel> _selectedMembers;

        #endregion

        #region Properties

        public ObservableCollection<PersonModel> AvailableMembers
        {
            get { return _availableMembers; }
            set { 
                _availableMembers = value;
                PropertyChanged?.Invoke(nameof(AvailableMembers), new PropertyChangedEventArgs(nameof(AvailableMembers)));
            }
        }

        public ObservableCollection<PersonModel> SelectedMembers
        {
            get { return _selectedMembers; }
            set 
            {
                _selectedMembers = value;
                PropertyChanged?.Invoke(nameof(SelectedMembers), new PropertyChangedEventArgs(nameof(AvailableMembers)));
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public CreateTeamViewModel()
        {
            var personModels = TournamentTracker.GlobalConfiguration.Connection.GetAllParticipants();
            AvailableMembers = new ObservableCollection<PersonModel>(personModels);

            SelectedMembers = new ObservableCollection<PersonModel>();
        }


        public void AddUserToTeam(PersonModel person)
        {
            AvailableMembers.Remove(person);
            CollectionChanged?.Invoke(nameof(CreateTeamViewModel), 
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));

            SelectedMembers.Add(person);
            CollectionChanged?.Invoke(nameof(CreateTeamViewModel),
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
        }

        public void SaveTeam()
        {
            SelectedMembers.Clear();
            CollectionChanged?.Invoke(nameof(CreateTeamViewModel),
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
        }
    }
}
