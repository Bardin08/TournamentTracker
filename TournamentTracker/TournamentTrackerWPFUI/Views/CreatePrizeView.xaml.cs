using System.Windows;
using MahApps.Metro.Controls;

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

        private void CreatePrizeButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
