using System.Windows;
using MahApps.Metro.Controls;

namespace TournamentTrackerWPFUI.Views
{
    /// <summary>
    /// Interact logic for TournamentDashboardView.xaml
    /// </summary>
    public partial class TournamentDashboardView : MetroWindow
    {
        public TournamentDashboardView()
        {
            InitializeComponent();
        }

        private void LoadTournamentButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CreateTournamentButton_Click(object sender, RoutedEventArgs e)
        {
            new CreateTournament().Show();
            Close();
        }
    }
}
