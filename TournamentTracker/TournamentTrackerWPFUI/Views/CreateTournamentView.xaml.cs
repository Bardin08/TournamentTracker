using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using TournamentTracker.Domain.Models;
using TournamentTrackerWPFUI.Helpers;
using TournamentTrackerWPFUI.Interfaces;
using TournamentTrackerWPFUI.ViewModels;

namespace TournamentTrackerWPFUI.Views
{
    /// <summary>
    /// Interaction logic for CreateTournament.xaml
    /// </summary>
    public partial class CreateTournament : MetroWindow, IPrizeRequester, ITeamRequester
    {
        private readonly CreateTournamentViewModel _viewModel = 
            new CreateTournamentViewModel();

        private readonly ITournamentRequester _caller;

        public CreateTournament(ITournamentRequester caller)
        {
            InitializeComponent();

            _caller = caller;

            DataContext = _viewModel;
        }

        #region UI Interaction methods

        private async void CreateTournamentButton_Click(object sender, RoutedEventArgs e)
        {
            var validationResult = ValidateTournament();
            if (validationResult.IsValid)
            {
                var tournament = CreateTournamentModel();

                TournamentTracker.Logic.TournamentLogic.CreateRounds(tournament);

                await Task.Run(() => TournamentTracker.GlobalConfiguration.Connection.SaveTournament(tournament));
                _caller.TournamentCreated(tournament);
                await this.ShowMessageAsync("", "Tournament successfully created");

                Close();
            }
            else
            {
                await this.ShowMessageAsync("Validation Error!", validationResult.Errors.GetValidationErrorMessage());
            }
        }

        private void AddTeamButton_Click(object sender, RoutedEventArgs e)
        {
            if ((TeamsListBox.SelectedItem as TeamModel) != null)
            {
                _viewModel.AddTeamToTournament(TeamsListBox.SelectedItem as TeamModel);
            }
        }

        private void AddPrizeButton_Click(object sender, RoutedEventArgs e)
        {
            if ((PrizesListBox.SelectedItem as PrizeModel) != null)
            {
                _viewModel.AddPrizeToTournament(PrizesListBox.SelectedItem as PrizeModel);
            }
        }

        private void CreateTeamButton_Click(object sender, RoutedEventArgs e)
        {
            new CreateTeamView(this).Show();
        }

        private void CreatePrizeButton_Click(object sender, RoutedEventArgs e)
        {
            new CreatePrizeView(this).Show();
        }

        #endregion

        private (bool IsValid, List<string> Errors) ValidateTournament()
        {
            var fieldsAreNotEmpty = !(new [] { TournamentNameTextBox.Text, EntryFeeTextBox.Text }.IsNullEmptyOrWhitespace());
            var atLeastTwoTeams = _viewModel.SelectedTeams.Count >= 2;
            var entryFeeIsValid = decimal.TryParse(EntryFeeTextBox.Text, out decimal fee) && fee >= 0; 

            List<string> errors = TournamentValidationResult(fieldsAreNotEmpty, atLeastTwoTeams, entryFeeIsValid);

            var isValid = fieldsAreNotEmpty && atLeastTwoTeams && entryFeeIsValid;

            return (isValid, errors);
        }

        private List<string> TournamentValidationResult(bool fieldsAreNotEmpty, bool atLeastTwoTeams, bool entryFeeIsValid)
        {
            var errors = new List<string>();

            if (!fieldsAreNotEmpty)
            {
                errors.Add("All fields must be filled");    
            }

            if (!atLeastTwoTeams)
            {
                errors.Add("At least two teams should be added");
            }

            if (!entryFeeIsValid)
            {
                errors.Add("Invalid entry fee");
            }

            return errors;
        }

        private TournamentModel CreateTournamentModel()
        {
            _viewModel.SelectedPrizes.ToList().RemoveAll(x => x == null);
            _viewModel.SelectedTeams.ToList().RemoveAll(x => x == null);
            return new TournamentModel
            {
                TournamentName = TournamentNameTextBox.Text,
                EntryFee = decimal.Parse(EntryFeeTextBox.Text),
                EnteredTeams = _viewModel.SelectedTeams.ToList(),
                Prizes = _viewModel.SelectedPrizes.ToList(),
                Rounds = new List<List<MatchModel>>()
            };
        }

        public void TeamCreated(TeamModel team)
        {
            _viewModel.CreateTeam(team);   
        }

        public void PrizeCreated(PrizeModel prize)
        {
            _viewModel.CreatePrize(prize);
        }
    }
}
