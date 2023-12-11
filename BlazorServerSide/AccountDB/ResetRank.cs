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
        public static async Task ResetRankAsync()
        {
            await using (var conn = new MySqlConnection(MyProjectInfoConfig.Instance.ConnectionString))
            {
                await SpResetRankAsync(conn, null);
            }
        }

        private static async Task SpResetRankAsync(MySqlConnection conn, MySqlTransaction trxn)
        {
            var parameters = new DynamicParameters();

            await conn.ExecuteAsync("call spResetRank();");
        }
    }
}




