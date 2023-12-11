using System.Data;
using BlazorApp3.Common;
using Common;
using Type;
using Dapper;
using MySqlConnector;

namespace BlazorApp3.Common
{
    public partial class AccountDB
    {
        public static async Task SetRankHistoryAsync(RankHistory rankHistory)
        {
            await using (var conn = new MySqlConnection(MyProjectInfoConfig.Instance.ConnectionString))
            {
                await SpSetRankHistoryAsync(conn, null, rankHistory);
            }
        }

        private static async Task SpSetRankHistoryAsync(MySqlConnection conn, MySqlTransaction trxn, RankHistory rankHistory)
        {
            var parameters = new DynamicParameters();
            parameters.Add("_userSeq", rankHistory.UserSeq);
            parameters.Add("_time", rankHistory.Time);
            parameters.Add("_ranking", rankHistory.Ranking);
            parameters.Add("_winRate", rankHistory.WinRate);

            await conn.ExecuteAsync("spSetRankHistory", parameters, trxn, commandType: CommandType.StoredProcedure);
        }
    }
}




