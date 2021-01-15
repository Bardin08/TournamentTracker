using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using TournamentTracker.Models;
using TournamentTrackerWPFUI.Helpers;

namespace TournamentTrackerWPFUI.Views
{
    /// <summary>
    /// Interact logic for CreateTeamView.xaml
    /// </summary>
    public partial class CreateTeamView : MetroWindow
    {
        public CreateTeamView()
        {
            InitializeComponent();
        }

        private async void AddTeamMemberButton_Click(object sender, RoutedEventArgs e)
        {
            var validationResult = ValidatePersonData();
            if (validationResult.IsValid)
            {
                var person = CreatePersonModel();
                await Task.Run(() => TournamentTracker.GlobalConfiguration.Connection.CreatePerson(person));

                ClearPersonData();
            }
            else
            {
                var sb = new StringBuilder("Errors:\n");

                foreach (var error in validationResult.Errors)
                {
                    sb.Append("  • ").Append(error).Append(";\n");
                }

                await this.ShowMessageAsync("Form validation result", sb.ToString());

            }
        }

        private void TeamMembersListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void TournamentParticipantsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

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
    }
}
