using System.Configuration;
using System.Windows;

using TournamentTracker;
using TournamentTracker.Data.Connectors;
using TournamentTracker.Notifiers;
using TournamentTracker.Notifiers.Options;

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

            // TODO: Add options and BL configuring from model
            TournamentTracker.GlobalConfiguration.InitConnections(new MSSQLConnector());

            var options = new TelegramNotifierOptions()
            {
                BotToken = ConfigurationManager.AppSettings["notifierTelegramBotToken"],
                ChatId = long.Parse(ConfigurationManager.AppSettings["chatId"])
            };

            GlobalConfiguration.AddNotificationSource(new TelegramNotifier(options));
        }
    }
}
