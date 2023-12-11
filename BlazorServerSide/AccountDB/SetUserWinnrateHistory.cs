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
        public static async Task SetUserWinRateHistoryAsync(UserWinRateHistory userWinRateHistory)
        {
            await using (var conn = new MySqlConnection(MyProjectInfoConfig.Instance.ConnectionString))
            {
                await SpSetUserWinRateHistoryAsync(conn, null, userWinRateHistory);
            }
        }

        private static async Task SpSetUserWinRateHistoryAsync(MySqlConnection conn, MySqlTransaction trxn, UserWinRateHistory userWinRateHistory)
        {
            var parameters = new DynamicParameters();
            parameters.Add("_userSeq", userWinRateHistory.UserSeq);
            parameters.Add("_lineType", userWinRateHistory.LineType);
            parameters.Add("_winCount", userWinRateHistory.WinCount);
            parameters.Add("_loseCount", userWinRateHistory.LoseCount);

            await conn.ExecuteAsync("spInsertUserWinnrateHistory", parameters, trxn, commandType: CommandType.StoredProcedure);
        }
    }
}




