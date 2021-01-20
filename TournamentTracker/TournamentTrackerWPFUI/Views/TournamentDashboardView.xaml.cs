using System.Windows;
using MahApps.Metro.Controls;
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

        private void CreateTournamentButton_Click(object sender, RoutedEventArgs e)
        {
            new CreateTournament(this).Show();
            Close();
        }

        public void TournamentCreated(TournamentModel tournament)
        {
            _viewModel.SelectedTournament = tournament;    
        }
    }
}
