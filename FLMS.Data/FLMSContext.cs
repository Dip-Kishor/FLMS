using FLMS.Data.SP_Result;
using FLMS.Models;
using FLMS.Models.FixturesAndResults;
using FLMS.Models.PlayerRegistration;
using FLMS.Models.User;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace FLMS.Data
{
    public class FLMSContext : DbContext
    {
        public FLMSContext(DbContextOptions<FLMSContext> options)
       : base(options)
        {
        }
        public DbSet<EUser> Users { get; set; }
        public DbSet<EUserRole> UserRoles { get; set; }
        public DbSet<ERegisteredPlayers> RegisteredPlayers { get; set; }
        public DbSet<ESeason> Seasons { get; set; }
        public DbSet<EFixturesAndResults> FixturesAndResults { get; set; }

        //public async Task<List<Table_SP_Results>> GetTableReportAsync(int seasonId)
        //{
        //    var results = new List<Table_SP_Results>();

        //    using (var command = this.Database.GetDbConnection().CreateCommand())
        //    {
        //        command.CommandText = "SP_Get_TableReport";
        //        command.CommandType = CommandType.StoredProcedure;

        //        // Add parameter for stored procedure
        //        var seasonParam = new SqlParameter("@SeasonId", SqlDbType.Int) { Value = seasonId };
        //        command.Parameters.Add(seasonParam);

        //        await this.Database.OpenConnectionAsync();

        //        using (var reader = await command.ExecuteReaderAsync())
        //        {
        //            while (await reader.ReadAsync())
        //            {
        //                results.Add(new Table_SP_Results
        //                {
        //                    Group = (GroupType)reader["Group"],
        //                    Rank = Convert.ToInt32(reader.GetInt64(reader.GetOrdinal("Rank"))),
        //                    PlayerId = reader.GetInt32(reader.GetOrdinal("PlayerId")),
        //                    PlayerName = reader.GetString(reader.GetOrdinal("PlayerName")),
        //                    MatchesPlayed = reader.GetInt32(reader.GetOrdinal("MatchesPlayed")),
        //                    Wins = reader.GetInt32(reader.GetOrdinal("Wins")),
        //                    Draws = reader.GetInt32(reader.GetOrdinal("Draws")),
        //                    Losses = reader.GetInt32(reader.GetOrdinal("Losses")),
        //                    GF = reader.GetInt32(reader.GetOrdinal("GF")),
        //                    GA = reader.GetInt32(reader.GetOrdinal("GA")),
        //                    GD = reader.GetInt32(reader.GetOrdinal("GD")),
        //                    Points = reader.GetInt32(reader.GetOrdinal("Points")),
        //                });
        //            }
        //        }
        //    }

        //    return results;
        //}
        public async Task<List<Table_SP_Results>> GetTableReportAsync(int seasonId)
        {
            var seasonIdParam = new SqlParameter("@SeasonId", seasonId);
            StoredPocedureExtension spEx = new StoredPocedureExtension(this);
            return await spEx.ExecuteStoredProcedureAsync<Table_SP_Results>("SP_Get_TableReport",
             seasonIdParam
                );
        }

    }
}
