using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using TournamentTracker;
using TournamentTracker.Models;
using TournamentTrackerWPFUI.Helpers;

namespace TournamentTrackerWPFUI.Views
{
    /// <summary>
    /// Interact logic for CreatePrizeView.xaml
    /// </summary>
    public partial class CreatePrizeView : MetroWindow
    {
        public CreatePrizeView()
        {
            InitializeComponent();
        }

        private async void CreatePrizeButton_Click(object sender, RoutedEventArgs e)
        {
            var validationResult = ValidateForm();
            if (validationResult.IsValid)
            {
                var model = CreatePrizeModelFromFrom();
                await Task.Run(() => GlobalConfiguration.Connection.CreatePrize(model));
            }
            else
            {
                var sb = new StringBuilder();

                sb.Append("Errors:\n");

                foreach (var error in validationResult.Errors)
                {
                    sb.Append("  • ").Append(error).Append(";\n");
                }

                await this.ShowMessageAsync("Form validation result", sb.ToString());
            }
        }

        private PrizeModel CreatePrizeModelFromFrom()
        {
            return new PrizeModel
            {
                PlaceName = PlaceNameTextBox.Text,
                PlaceNumber = int.Parse(PlaceNumberTextBox.Text),
                PrizePercentage = double.Parse(PrizePercentageTextBox.Text),
                PrizeAmount = decimal.Parse(PrizeAmountTextBox.Text),
                PrizeName = PrizeNameTextBox.Text
            };
        }

        private (bool IsValid, List<string> Errors) ValidateForm()
        {
            var containsUnvalidStrings = new string[] {
                PlaceNameTextBox.Text,
                PlaceNumberTextBox.Text,
                PrizeAmountTextBox.Text,
                PrizeNameTextBox.Text,
                PrizePercentageTextBox.Text
            }.IsNullEmptyOrWhitespace();

            decimal prizeAmount = 0;
            double prizePercentage = 0;

            var containsAmountPlaceAndPercentage =
                int.TryParse(PlaceNumberTextBox.Text, out int placeNumber) &&
                decimal.TryParse(PrizeAmountTextBox.Text, out prizeAmount) &&
                double.TryParse(PrizePercentageTextBox.Text, out prizePercentage);

            var percentageOrAmountIsZero =
                ((((int)prizeAmount) != 0) && (((int)prizePercentage) == 0)) ||
                ((((int)prizeAmount) == 0) && (((int)prizePercentage) != 0));

            var isValid = !containsUnvalidStrings &&
                containsAmountPlaceAndPercentage &&
                percentageOrAmountIsZero &&
                placeNumber > 0 &&
                prizePercentage >= 0 &&
                prizeAmount >= 0;

            List<string> errors = GenerateErrorMessage(
                containsUnvalidStrings,
                prizeAmount,
                prizePercentage,
                containsAmountPlaceAndPercentage,
                placeNumber,
                percentageOrAmountIsZero);

            return (IsValid: isValid, Errors: errors);
        }

        private static List<string> GenerateErrorMessage(bool containsUnvalidStrings, decimal prizeAmount,
            double prizePercentage, bool containsAmountPlaceAndPercentage, int placeNumber, bool percentageOrAmountIsZero)
        {
            var errors = new List<string>();

            if (containsUnvalidStrings)
            {
                errors.Add("From contains one or more invalid strings");
            }

            if (!containsAmountPlaceAndPercentage)
            {
                errors.Add("From should contains amount, place and percentage");
            }

            if (!percentageOrAmountIsZero)
            {
                errors.Add("Percentage or amount should equals to zero");
            }

            if (placeNumber <= 0)
            {
                errors.Add("PlaceNumber must be equals or higher than one");
            }

            if (prizeAmount < 0)
            {
                errors.Add("Prize amount can`t be less then zero");
            }

            if (prizePercentage < 0)
            {
                errors.Add("PrizePercentage can`t be less then zero");
            }
            
            return errors;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
