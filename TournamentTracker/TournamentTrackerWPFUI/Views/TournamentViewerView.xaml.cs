using MahApps.Metro.Controls;
using TournamentTracker.Models;

namespace TournamentTrackerWPFUI.Views
{
    /// <summary>
    /// Interacting logic for TournamentViewerView.xaml
    /// </summary>
    public partial class TournamentViewerView : MetroWindow
    {
        public TournamentViewerView(TournamentModel tournament)
        {
            InitializeComponent();
        }

        private void SubmitScoreButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void MatchesListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void OnlyUnplayedMatchesCheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void RoundSplitButton_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
