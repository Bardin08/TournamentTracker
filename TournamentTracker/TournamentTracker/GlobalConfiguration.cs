using TournamentTracker.Interfaces;

namespace TournamentTracker
{
    /// <summary>
    /// Global configuration class. Gives access to global variables and objects. 
    /// </summary>
    public static class GlobalConfiguration
    {
        /// <summary>
        /// Connector for access to the data storage. 
        /// </summary>
        public static IDataConnection Connection { get; private set; }

        /// <summary>
        /// Initiate connector. Allows changing data storage. 
        /// </summary>
        /// <param name="connection"> A data storage connector. </param>
        public static void InitConnections(IDataConnection connection)
        {
            if (connection == null)
            {
                throw new System.ArgumentException($"\"{nameof(connection)}\" can`t be null", nameof(connection));
            }

            Connection = connection;
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
