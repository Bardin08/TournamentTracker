using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Linq;
using TournamentTracker;
using TournamentTracker.Models;
using TournamentTrackerWPFUI.ViewModels;

namespace TournamentTrackerWPFUI.Views
{
    /// <summary>
    /// Interaction logic for TournamentViewerView.xaml
    /// </summary>
    public partial class TournamentViewerView : MetroWindow
    {
        private readonly TournamentViewerViewModel _viewModel;

        public TournamentViewerView(TournamentModel tournament)
        {
            InitializeComponent();

            _viewModel = new TournamentViewerViewModel(GlobalConfiguration.Connection, tournament);

            DataContext = _viewModel;
        }

        private void RoundComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _viewModel.CurrentRound = (int)RoundComboBox.SelectedItem;
        }

        private void MatchesListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _viewModel.SelectedMatch = (MatchesListBox.SelectedItem as MatchModel);
        }

        private void ShowOnlyUnplayedCheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.ShowOnlyUnplayed = true;
        }

        private void ShowOnlyUnplayedCheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.ShowOnlyUnplayed = false;
        }

        private async void SubmitScoreButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ValidateScore())
            {
                int firstTeamScore = int.Parse(FirstTeamScoreTextBox.Text);
                int secondTeamScore = int.Parse(SecondTeamScoreTextBox.Text);

                _viewModel.WriteMatchResult(firstTeamScore, secondTeamScore);
            }
            else
            {
                await this.ShowMessageAsync("", "Incorrect score");
            }
        }

        private bool ValidateScore()
        {
            var teamsScoreCorrect = int.TryParse(FirstTeamScoreTextBox.Text, out int firstTeamScore) &&
                int.TryParse(SecondTeamScoreTextBox.Text, out int secondTeamScore) &&
                firstTeamScore >= 0 && 
                secondTeamScore >= 0 &&
                firstTeamScore != secondTeamScore;

            return teamsScoreCorrect;
        }
    }
}
