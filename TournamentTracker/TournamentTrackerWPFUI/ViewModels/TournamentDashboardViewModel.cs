using TournamentTracker.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace TournamentTrackerWPFUI.ViewModels
{
    public class TournamentDashboardViewModel : INotifyCollectionChanged
    {
        private ObservableCollection<TournamentModel> _tournaments;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public ObservableCollection<TournamentModel> Tournaments
        {
            get { return _tournaments; }
            set 
            {
                _tournaments = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public TournamentModel SelectedTournament { get; set; }

        public TournamentDashboardViewModel()
        {
            // TODO: Realize tournament loading
            Tournaments =
                new ObservableCollection<TournamentModel>(TournamentTracker.GlobalConfiguration.Connection.GetTournaments());

            SelectedTournament = Tournaments[0];
        }
    }
}
