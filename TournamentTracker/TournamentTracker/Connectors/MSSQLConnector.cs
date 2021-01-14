using Dapper;
using System.Data;
using System.Data.SqlClient;
using TournamentTracker.Interfaces;
using TournamentTracker.Models;

namespace TournamentTracker.Connectors
{
    public class MSSQLConnector : IDataConnection
    {
        public PrizeModel CreatePrize(PrizeModel prize)
        {
            using (var connection = 
                new SqlConnection(GlobalConfiguration.GetConnectionString("Tournaments")))
            {
                var p = new Dapper.DynamicParameters();

                p.Add("@PlaceNumber", prize.PlaceNumber);
                p.Add("@PlaceName", prize.PlaceName);
                p.Add("@PrizeName", prize.PrizeName);
                p.Add("@PrizeAmount", prize.PrizeAmount);
                p.Add("@PrizePercentage", prize.PricePercentage);
                p.Add("@id", prize.Id, DbType.Int32, ParameterDirection.Output);

                connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                prize.Id = p.Get<int>("@id");

                return prize;
            }
        }
    }
}
