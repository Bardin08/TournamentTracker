using System;
using System.Collections.Generic;

using TournamentTracker.Interfaces;

namespace TournamentTracker
{
    /// <summary>
    /// Global configuration class. Gives access to global variables and objects. 
    /// </summary>
    public static class GlobalConfiguration
    {
        #region Text file names

        public const string PrizesFileName = "PrizeModels.csv";
        public const string PeopleFileName = "PeopleModels.csv";
        public const string TeamsFileName = "TeamModels.csv";
        public const string TournamentsFileName = "TournamentsModels.csv";
        public const string MatchesFileName = "Matchups.csv";
        public const string MatchEntriesFileName = "MatchupEntries.csv";

        #endregion

        /// <summary>
        /// Connector for access to the data storage. 
        /// </summary>
        public static IDataConnection Connection { get; private set; }

        /// <summary>
        /// Sources where notifications should be sent.
        /// </summary>
        public static List<INotificationSource> NotificationSources { get; private set; } = new List<INotificationSource>();

        /// <summary>
        /// Initiate connector. Allows changing data storage. 
        /// </summary>
        /// <param name="connection"> A data storage connector. </param>
        public static void InitConnections(IDataConnection connection)
        {
            Connection = connection ?? throw new System.ArgumentException($"\"{nameof(connection)}\" can`t be null", nameof(connection));
        }

        public static void AddNotificationSource(INotificationSource notificationSource)
        {
            if (notificationSource == null)
            {
                throw new ArgumentNullException(nameof(notificationSource));
            }

            NotificationSources.Add(notificationSource);
        }

        /// <summary>
        /// Return the connector string.
        /// </summary>
        /// <param name="name"> Connection string name. </param>
        public static string GetConnectionString(string name)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
