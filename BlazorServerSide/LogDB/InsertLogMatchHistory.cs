using System.Data;
using BlazorApp3.Common;
using Common;
using Type;
using Dapper;
using MySqlConnector;

namespace BlazorApp3.Common
{
    public partial class LogDB
    {
        public static async Task InsertLogMatchHistoryAsync(LogMatchHistory matchHistory)
        {
            await using (var conn = new MySqlConnection(MyProjectInfoConfig.Instance.ConnectionString))
            {
                await SpInsertLogMatchHistoryAsync(conn, null, matchHistory);
            }
        }

        private static async Task SpInsertLogMatchHistoryAsync(MySqlConnection conn, MySqlTransaction trxn, LogMatchHistory matchHistory)
        {
            var parameters = new DynamicParameters();
            parameters.Add("_team1Data", matchHistory.Team1Data);
            parameters.Add("_team2Data", matchHistory.Team2Data);
            parameters.Add("_team1Win", matchHistory.Team1Win);
            parameters.Add("_team2Win", matchHistory.Team2Win);

            await conn.ExecuteAsync("spInsertLogMatchHistory", parameters, trxn, commandType: CommandType.StoredProcedure);
        }
    }
}




