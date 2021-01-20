using System.Windows;
using TournamentTracker.Connectors;

namespace TournamentTrackerWPFUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            TournamentTracker.GlobalConfiguration.InitConnections(new MSSQLConnector());
        }
    }
}
