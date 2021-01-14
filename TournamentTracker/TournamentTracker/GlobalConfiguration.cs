using System.Collections.Generic;
using TournamentTracker.Interfaces;

namespace TournamentTracker
{
    public static class GlobalConfiguration
    {
        public static List<IDataConnection> Connections { get; private set; } = new List<IDataConnection>();

        public static void InitConnections(List<IDataConnection> connections)
        {
            if (connections == null)
            {
                throw new System.ArgumentException($"\"{nameof(connections)}\" can`t be null", nameof(connections));
            }

            connections.AddRange(connections);
        }
    }
}
