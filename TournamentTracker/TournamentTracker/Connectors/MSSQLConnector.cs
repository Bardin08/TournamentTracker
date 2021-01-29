using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;

using Dapper;

using TournamentTracker.Models;
using TournamentTracker.Interfaces;

namespace TournamentTracker.Connectors
{
    /// <summary>
    /// Connector for MS SQL database. Allows saving data to MS SQL databases.
    /// </summary>
    public class MSSQLConnector : IDataConnection
    {
        private const string DatabaseName = "Tournaments";

        public PersonModel SavePerson(PersonModel person)
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

        public PrizeModel SavePrize(PrizeModel prize)
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

        public TeamModel SaveTeam(TeamModel team)
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

        public TournamentModel SaveTournament(TournamentModel tournament)
        {
            using (var connection = new SqlConnection(GlobalConfiguration.GetConnectionString(DatabaseName)))
            {
                tournament = SaveTournament(tournament, connection);
                SaveTournamentPrizes(tournament, connection);
                SaveTournamentTeams(tournament, connection);
                SaveTournamentRounds(tournament, connection);
            }
            return tournament;
        }

        private void SaveTournamentRounds(TournamentModel tournament, SqlConnection connection)
        {
            foreach (var round in tournament.Rounds)
            {
                foreach (var match in round)
                {
                    var p = new DynamicParameters();

                    p.Add("@RoundNumber", match.RoundNumber);
                    p.Add("@TournamentId", tournament.Id);
                    p.Add("@id", 0, DbType.Int32, ParameterDirection.Output);

                    connection.Execute("dbo.spMatches_Insert", p, commandType: CommandType.StoredProcedure);

                    match.Id = p.Get<int>("@id");

                    foreach (var matchEntry in match.Entries)
                    {
                        SaveMatchEntry(connection, match, matchEntry);
                    }
                }
            }
        }

        private void SaveMatchEntry(SqlConnection connection, MatchModel match, MatchEntryModel matchEntry)
        {
            DynamicParameters p = new DynamicParameters();

            p.Add("@MatchId", match.Id);

            if (matchEntry.ParentMatch == null)
            {
                p.Add("@ParentMatchId", null);
            }
            else
            {
                p.Add("@ParentMatchId", matchEntry.ParentMatch.Id);
            }

            if (matchEntry.CompetingTeam == null)
            {
                p.Add("@TeamCompetitingId", null);
            }
            else
            {
                p.Add("@TeamCompetitingId", matchEntry.CompetingTeam.Id);
            }
            p.Add("@id", 0, DbType.Int32, ParameterDirection.Output);

            connection.Execute("dbo.spMatchEntries_Insert", p, commandType: CommandType.StoredProcedure);
        }

        public List<PersonModel> GetAllParticipants()
        {
            using (var connection = new SqlConnection(GlobalConfiguration.GetConnectionString(DatabaseName)))
            {
                return connection.Query<PersonModel>("dbo.spPeople_GetAll").ToList();
            }
        }

        public List<PrizeModel> GetPrizes()
        {
            using (var connection = new SqlConnection(GlobalConfiguration.GetConnectionString(DatabaseName)))
            {
                return connection.Query<PrizeModel>("dbo.spPrizes_GetAll").ToList();
            }
        }
        
        // TODO: Add participants to teams
        public List<TeamModel> GetTeams()
        {
            using (var connection = new SqlConnection(GlobalConfiguration.GetConnectionString(DatabaseName)))
            {
                return connection.Query<TeamModel>("dbo.spTeams_GetAll").ToList();
            }
        }

        private static void SaveTournamentTeams(TournamentModel tournament, SqlConnection connection)
        {
            foreach (var team in tournament.EnteredTeams)
            {
                if (team != null)
                {
                    var p = new DynamicParameters();

                    p.Add("@TournamentId", tournament.Id);
                    p.Add("@TeamId", team.Id);

                    connection.Execute("dbo.spTournamentEntries_Insert", p, commandType: CommandType.StoredProcedure);
                }
            }
        }

        private static void SaveTournamentPrizes(TournamentModel tournament, SqlConnection connection)
        {
            foreach (var prize in tournament.Prizes)
            {
                if (prize != null)
                {
                    var p = new DynamicParameters();

                    p.Add("@TournamentId", tournament.Id);
                    p.Add("@PrizeId", prize.Id);

                    connection.Execute("dbo.spTournamentPrizes_Insert", p, commandType: CommandType.StoredProcedure);
                }
            }
        }

        private static TournamentModel SaveTournament(TournamentModel tournament, SqlConnection connection)
        {
            var p = new DynamicParameters();

            p.Add("@TournamentName", tournament.TournamentName);
            p.Add("@EntryFee", tournament.EntryFee);
            p.Add("@id", 0, DbType.Int32, ParameterDirection.Output);

            connection.Execute("dbo.spTournaments_Insert", p, commandType: CommandType.StoredProcedure);

            tournament.Id = p.Get<int>("@id");
            return tournament;
        }

        // TODO: Refactoring requires
        public List<TournamentModel> GetTournaments()
        {
            using (var connection = new SqlConnection(GlobalConfiguration.GetConnectionString(DatabaseName)))
            {
                var output = connection.Query<TournamentModel>("dbo.spTournaments_GetAll").ToList();

                foreach (var t in output)
                {
                    var p = new DynamicParameters();

                    p.Add("@TournamentId", t.Id);

                    t.EnteredTeams = connection.Query<TeamModel>("spTeams_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();

                    foreach (var tm in t.EnteredTeams)
                    {
                        p = new DynamicParameters();

                        p.Add("@TeamId", tm.Id);

                        tm.TeamMembers = connection.Query<PersonModel>("spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
                    }

                    p = new DynamicParameters();

                    p.Add("@TournamentId", t.Id);

                    t.Prizes = connection.Query<PrizeModel>("spPrizes_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();

                    var matches = connection.Query<MatchModel>("spMatch_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();
                    if (matches.Count > 0)
                    {
                        int rounds = matches.OrderByDescending(x => x.RoundNumber).First().RoundNumber;

                        for (int i = 1; i <= rounds; i++)
                        {
                            t.Rounds.Add(matches.Where(x => x.RoundNumber == i).ToList());
                        }

                        foreach (var r in t.Rounds)
                        {
                            foreach (var m in r)
                            {
                                p = new DynamicParameters();

                                p.Add("@MatchId", m.Id);

                                m.Entries = connection.Query<MatchEntryModel>("spMatchEntries_GetByMatch", p, commandType: CommandType.StoredProcedure).ToList();

                                m.Winner = t.EnteredTeams.FirstOrDefault(team => team.Id == m.WinnerId);

                                foreach (var e in m.Entries)
                                {
                                    if (e.TeamCompetingId != null)
                                    {
                                        e.CompetingTeam = t.EnteredTeams.First(t => t.Id == e.TeamCompetingId);
                                    }
                                }
                            }
                        }
                    }
                }
                return output;
            }
        }

        public MatchModel UpdateMatch(MatchModel match)
        {
            using (var connection = new SqlConnection(GlobalConfiguration.GetConnectionString(DatabaseName)))
            {
                var p = new DynamicParameters();

                p.Add("@MatchId", match.Id);
                p.Add("@WinnerId", match.Winner.Id);

                connection.Execute("dbo.spMatches_Update", p, commandType: CommandType.StoredProcedure);

                foreach (var me in match.Entries)
                {
                    p = new DynamicParameters();

                    p.Add("@MatchEntryId", me.Id);
                    p.Add("@TeamCompetingId", me.TeamCompetingId);
                    p.Add("@Score", me.Score);

                    connection.Execute("dbo.spMatchEntries_Update", p, commandType: CommandType.StoredProcedure);
                }
            }

            return match;
        }
    }
}
