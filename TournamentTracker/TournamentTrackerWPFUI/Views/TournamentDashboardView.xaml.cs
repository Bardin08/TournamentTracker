using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using TournamentTracker;
using TournamentTracker.Models;
using TournamentTrackerWPFUI.Interfaces;
using TournamentTrackerWPFUI.ViewModels;

namespace TournamentTrackerWPFUI.Views
{
    /// <summary>
    /// Interact logic for TournamentDashboardView.xaml
    /// </summary>
    public partial class TournamentDashboardView : MetroWindow, ITournamentRequester
    {
        private readonly TournamentDashboardViewModel _viewModel = new TournamentDashboardViewModel();

        public TournamentDashboardView()
        {
            InitializeComponent();

            DataContext = _viewModel;
        }

        private void LoadTournamentButton_Click(object sender, RoutedEventArgs e)
        {
            new TournamentViewerView(_viewModel.SelectedTournament).Show();
            Close();
        }

        private async void CreateTournamentButton_Click(object sender, RoutedEventArgs e)
        {
            new CreateTournament(this).Show();


            // TODO: Check this
            _viewModel.Tournaments = new System.Collections.ObjectModel.ObservableCollection<TournamentModel>
                (await Task.Run(() => GlobalConfiguration.Connection.GetTournaments()));
        }

        public void TournamentCreated(TournamentModel tournament)
        {
            _viewModel.SelectedTournament = tournament;    
        }

        private void TeamSplitButton_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _viewModel.SelectedTournament = TeamSplitButton.SelectedItem as TournamentModel;
        }
    }
}
