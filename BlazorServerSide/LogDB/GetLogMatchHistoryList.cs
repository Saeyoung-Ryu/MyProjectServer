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
        public static async Task<List<LogMatchHistory>> GetLogMatchHistoryAsync()
        {
            await using (var conn = new MySqlConnection(MyProjectInfoConfig.Instance.ConnectionString))
            {
                return await SpGetLogMatchHistoryAsync(conn, null);
            }
        }

        private static async Task<List<LogMatchHistory>> SpGetLogMatchHistoryAsync(MySqlConnection conn, MySqlTransaction trxn)
        {
            var parameters = new DynamicParameters();

            return (await conn.QueryAsync<LogMatchHistory>("spGetLogMatchHistoryList", parameters, trxn, commandType: CommandType.StoredProcedure)).ToList();
        }
    }
}




