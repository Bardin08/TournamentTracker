using MahApps.Metro.Controls;
using TournamentTracker.Models;
using TournamentTrackerWPFUI.ViewModels;

namespace TournamentTrackerWPFUI.Views
{
    /// <summary>
    /// Interacting logic for TournamentViewerView.xaml
    /// </summary>
    public partial class TournamentViewerView : MetroWindow
    {
        private readonly TournamentViewverViewModel _viewModel;
 
        public TournamentViewerView(TournamentModel tournament)
        {
            InitializeComponent();

            _viewModel = new TournamentViewverViewModel(tournament);

            DataContext = _viewModel;
        }

        private void SubmitScoreButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void MatchesListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _viewModel.SelectMatch(MatchesListBox.SelectedItem as MatchModel);
        }

        private void OnlyUnplayedMatchesCheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.ShowOnlyUnplayed((bool)OnlyUnplayedMatchesCheckBox.IsChecked);
        }

        private void RoundSplitButton_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }
    }
}
