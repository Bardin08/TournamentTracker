using System.Collections.Generic;
using TournamentTracker.Interfaces;

namespace TournamentTracker
{
    public static class GlobalConfiguration
    {
        public static IDataConnection Connection { get; private set; }

        public static void InitConnections(IDataConnection connection)
        {
            if (connection == null)
            {
                throw new System.ArgumentException($"\"{nameof(connection)}\" can`t be null", nameof(connection));
            }

            Connection = connection;
        }

        public static string GetConnectionString(string name)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
