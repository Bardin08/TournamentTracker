using System.Linq;
using System.Collections.Generic;
using TournamentTracker.Models;
using TournamentTracker.Interfaces;
using TournamentTracker.Connectors.TextHelpers;

namespace TournamentTracker.Connectors
{
    /// <summary>
    /// Connector for CSV file. Allows saving data to CSV files.
    /// </summary>
    public class CSVConnector : IDataConnection
    {
        public const string PrizesFilePath = "PrizeModels.csv";

        public PrizeModel CreatePrize(PrizeModel prize)
        {
            List<PrizeModel> prizes = PrizesFilePath.GetFilePath().LoadFile().ConvertLinesToPrizeModels();
            var lastPrizeId = 0;
            
            if (prizes.Count > 0)
            {
                lastPrizeId = prizes.OrderByDescending(x => x.Id).First().Id;
            }

            prize.Id = lastPrizeId + 1;

            prizes.Add(prize);
            prizes.SaveToPrizeFile(PrizesFilePath);

            return prize;
        }
    }
}
