using System;
using System.Linq;
using System.Windows;
using System.Net.Mail;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using TournamentTracker.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MahApps.Metro.Controls.Dialogs;
using TournamentTrackerWPFUI.Helpers;
using TournamentTrackerWPFUI.Interfaces;
using TournamentTrackerWPFUI.ViewModels;

namespace TournamentTrackerWPFUI.Views
{
    /// <summary>
    /// Interact logic for CreateTeamView.xaml
    /// </summary>
    public partial class CreateTeamView : MetroWindow
    {
        private readonly ITeamRequester _caller;

        public CreateTeamView(ITeamRequester caller)
        {
            InitializeComponent();

            _caller = caller;

            DataContext = new CreateTeamViewModel();
        }

        private async void AddTeamMemberButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPerson = TournamentParticipantsListBox.SelectedValue as PersonModel;

            if (selectedPerson != null)
            {
                var viewModel = DataContext as CreateTeamViewModel;

                viewModel.AddUserToTeam(selectedPerson);
            }
            else
            {
                await this.ShowMessageAsync("Error!", "You should select a tournament participant first.");
            }
        }

        private async void AddParticipantButton_Click(object sender, RoutedEventArgs e)
        {
            var validationResult = ValidatePersonData();
            if (validationResult.IsValid)
            {
                var person = CreatePersonModel();
                await Task.Run(() => person = TournamentTracker.GlobalConfiguration.Connection.SavePerson(person));

                (DataContext as CreateTeamViewModel).AvailableMembers.Add(person);
                
                ClearPersonData();
            }
            else
            {
                await this.ShowMessageAsync("Validation Error!", validationResult.Errors.GetValidationErrorMessage());
            }
        }

        #region Person creation methods

        private (bool IsValid, List<string> Errors) ValidatePersonData()
        {
            var allStringsNotEmpty = !(new string[]
            {
                FirstNameTextBox.Text,
                LastNameTextBox.Text,
                EmailAddressTextBox.Text,
                CellphoneNumberTextBox.Text
            }.IsNullEmptyOrWhitespace());

            bool validPhoneNumber = false, validEmailAddress = false;

            if (allStringsNotEmpty)
            {
                validPhoneNumber = ValidateCellphoneNumber(CellphoneNumberTextBox.Text);
                validEmailAddress = ValidateEmailAddress(EmailAddressTextBox.Text);
            }

            var isValid = allStringsNotEmpty &&
                validPhoneNumber &&
                validEmailAddress;

            var errors = PersonValidationResult(allStringsNotEmpty, validPhoneNumber, validEmailAddress);

            return (isValid, errors);
        }

        private bool ValidateCellphoneNumber(string cellphoneNumber)
        {
            return Regex.IsMatch(cellphoneNumber, @"\+[0-9]+\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
        }

        private bool ValidateEmailAddress(string address)
        {
            try
            {
                var m = new MailAddress(address);
                return true;
            }
            catch (FormatException) 
            {
                return false;
            }
        }

        private List<string> PersonValidationResult(bool allStringsNotEmpty, bool validPhoneNumber, bool validEmailAddress)
        {
            var errors = new List<string>();

            if (!allStringsNotEmpty)
            {
                errors.Add("All personal information must be entered.");
            }

            if (!validPhoneNumber)
            {
                errors.Add("Incorrect format number. Required format: +*(XXX)-XXX-XXXX");
            }

            if (!validEmailAddress)
            {
                errors.Add("Incorrect email address.");
            }

            return errors;
        }

        private PersonModel CreatePersonModel()
        {
            return new PersonModel(FirstNameTextBox.Text, LastNameTextBox.Text, EmailAddressTextBox.Text, CellphoneNumberTextBox.Text);
        }

        private void ClearPersonData()
        {
            FirstNameTextBox.Text = 
                LastNameTextBox.Text = 
                EmailAddressTextBox.Text = 
                CellphoneNumberTextBox.Text = "";
        }

        #endregion

        private async void CreateTeamButton_Click(object sender, RoutedEventArgs e)
        {
            var teamValidationResult = ValidateTeam();
            if (teamValidationResult.IsValid)
            {
                var team = CreateTeamModel();
                
                await Task.Run(() => TournamentTracker.GlobalConfiguration.Connection.SaveTeam(team));
                _caller.TeamCreated(team);

                (DataContext as CreateTeamViewModel).SaveTeam();
                Close();
            }
            else
            {
                await this.ShowMessageAsync("Validation Error!", teamValidationResult.Errors.GetValidationErrorMessage());
            }
        }

        private TeamModel CreateTeamModel()
        {
            return new TeamModel
            {
                TeamName = TeamNameTextBox.Text,
                TeamMembers = (DataContext as CreateTeamViewModel).SelectedMembers.ToList()
            };
        }

        private (bool IsValid, List<string> Errors) ValidateTeam()
        {
            var teamNotEmpty = (DataContext as CreateTeamViewModel).SelectedMembers.Count > 0;
            var teamNameIsNotEmpty = !(new string[] { TeamNameTextBox.Text }.IsNullEmptyOrWhitespace());
            var isValid = teamNotEmpty && teamNameIsNotEmpty;

            List<string> errors = TeamValidationResults(teamNameIsNotEmpty, teamNotEmpty);

            return (isValid, errors);
        }

        private List<string> TeamValidationResults(bool teamNameIsNotEmpty, bool teamNotEmpty)
        {
            var errors = new List<string>();

            if (!teamNotEmpty)
            {
                errors.Add("Team can`t be empty. You should add at least one team members.");
            }
            
            if (!teamNameIsNotEmpty)
            {
                errors.Add("Team name can`t be empty.");
            }

            return errors;
        }
    }
}
