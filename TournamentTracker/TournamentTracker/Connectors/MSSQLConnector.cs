﻿using Dapper;
using System.Data;
using System.Data.SqlClient;
using TournamentTracker.Interfaces;
using TournamentTracker.Models;

namespace TournamentTracker.Connectors
{
    /// <summary>
    /// Connector for MS SQL database. Allows saving data to MS SQL databases.
    /// </summary>
    public class MSSQLConnector : IDataConnection
    {
        public PersonModel CreatePerson(PersonModel person)
        {
            using (var connection =
                new SqlConnection(GlobalConfiguration.GetConnectionString("Tournaments"))) 
            {
                var p = new Dapper.DynamicParameters();

                p.Add("@FirstName", person.FirstName);
                p.Add("@LastName", person.LastName);
                p.Add("@EmailAddress", person.EmailAddress);
                p.Add("@CellphoneNumber", person.CellphoneNumber);
                p.Add("@id", person.Id, DbType.Int32, ParameterDirection.Output);

                connection.Execute("dbo.spPeople_Insert", p, commandType: CommandType.StoredProcedure);

                person.Id = p.Get<int>("@id");

                return person;
            }
        }

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
                p.Add("@PrizePercentage", prize.PrizePercentage);
                p.Add("@id", prize.Id, DbType.Int32, ParameterDirection.Output);

                connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                prize.Id = p.Get<int>("@id");

                return prize;
            }
        }
    }
}
