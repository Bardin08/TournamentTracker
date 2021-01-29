using System.ComponentModel;
using System.Collections.ObjectModel;

using TournamentTracker.Domain.Models;

namespace TournamentTrackerWPFUI.ViewModels
{
    public class TournamentDashboardViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TournamentModel> _tournaments;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TournamentModel> Tournaments
        {
            get { return _tournaments; }
            set 
            {
                _tournaments = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tournaments)));
            }
        }

        public TournamentModel SelectedTournament { get; set; }

        public TournamentDashboardViewModel()
        {
            Tournaments =
                new ObservableCollection<TournamentModel>(TournamentTracker.GlobalConfiguration.Connection.GetTournaments());

            SelectedTournament = Tournaments[0];
        }
    }
}
