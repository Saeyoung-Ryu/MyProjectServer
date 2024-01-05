using System.Data;
using Common;
using Enum;
using Type;
using Dapper;
using MySqlConnector;

namespace BlazorApp3.Common
{
    public partial class AccountDB
    {
        public static async Task<List<UserWinRateHistory>> GetUserWinRateHistoryAsync(int seq)
        {
            await using (var conn = new MySqlConnection(MyProjectInfoConfig.Instance.ConnectionString))
            {
                return await SpGetUserWinRateHistoryAsync(conn, null, seq);
            }
        }

        private static async Task<List<UserWinRateHistory>> SpGetUserWinRateHistoryAsync(MySqlConnection conn, MySqlTransaction trxn, int seq)
        {
            var parameters = new DynamicParameters();
            parameters.Add("_userSeq", seq);

            return (await conn.QueryAsync<UserWinRateHistory>("spGetUserWinRateHistory", parameters, trxn, commandType: CommandType.StoredProcedure)).ToList();
        }

        public static async Task<List<UserWinRateHistory>> GetAllUserWinRateHistoryByLine(LineType lineType)
        {
            await using (var conn = new MySqlConnection(MyProjectInfoConfig.Instance.ConnectionString))
            {
                return (await conn.QueryAsync<UserWinRateHistory>($"select * from tblUserWinnrateHistory where lineType = {(int) lineType}")).ToList();
            }
        }
        
        public static async Task<List<UserWinRateHistory>> GetAllUserWinRateHistory()
        {
            await using (var conn = new MySqlConnection(MyProjectInfoConfig.Instance.ConnectionString))
            {
                return (await conn.QueryAsync<UserWinRateHistory>($"select * from tblUserWinnrateHistory")).ToList();
            }
        }
    }
}




