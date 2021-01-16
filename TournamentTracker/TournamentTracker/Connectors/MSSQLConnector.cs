using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TournamentTracker.Interfaces;
using TournamentTracker.Models;

namespace TournamentTracker.Connectors
{
    /// <summary>
    /// Connector for MS SQL database. Allows saving data to MS SQL databases.
    /// </summary>
    public class MSSQLConnector : IDataConnection
    {
        private const string DatabaseName = "Tournaments";
        
        public PersonModel CreatePerson(PersonModel person)
        {
            using (var connection =
                new SqlConnection(GlobalConfiguration.GetConnectionString(DatabaseName))) 
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
                new SqlConnection(GlobalConfiguration.GetConnectionString(DatabaseName)))
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

        public TeamModel CreateTeam(TeamModel team)
        {
            using (var connection = new SqlConnection(GlobalConfiguration.GetConnectionString(DatabaseName)))
            {
                var p = new DynamicParameters();

                p.Add("@TeamName", team.TeamName);
                p.Add("@id", 0, DbType.Int32, ParameterDirection.Output);

                connection.Execute("dbo.spTeams_Insert", p, commandType: CommandType.StoredProcedure);

                team.Id = p.Get<int>("@id");

                foreach (var person in team.TeamMembers)
                {
                    p = new DynamicParameters();

                    p.Add("@TeamId", team.Id);
                    p.Add("@PersonId", person.Id);

                    connection.Execute("dbo.spTeamMembers_Insert", p, commandType: CommandType.StoredProcedure);
                }

                return team;
            }
        }

        public List<PersonModel> GetAllParticipants()
        {
            using (var connection = new SqlConnection(GlobalConfiguration.GetConnectionString(DatabaseName))) 
            {
                return connection.Query<PersonModel>("dbo.spPeople_GetAll").ToList();
            }
        }
    }
}
