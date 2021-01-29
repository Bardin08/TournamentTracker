namespace TournamentTracker.Data
{
    public static class DataConfiguration
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
        /// Return the connector string.
        /// </summary>
        /// <param name="name"> Connection string name. </param>
        public static string GetConnectionString(string name)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

    }
}