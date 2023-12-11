using System.Data;
using BlazorApp3.Common;
using Common;
using Dapper;
using MySqlConnector;

namespace BlazorApp3.Common
{
    public partial class AccountDB
    {
        public static async Task UpdateUserInfoAsync(string nickName, int seq)
        {
            await using (var conn = new MySqlConnection(MyProjectInfoConfig.Instance.ConnectionString))
            {
                await SpUpdateUserInfoAsync(conn, null, nickName, seq);
            }
        }

        private static async Task SpUpdateUserInfoAsync(MySqlConnection conn, MySqlTransaction trxn, string nickName, int seq)
        {
            var parameters = new DynamicParameters();
            parameters.Add("_userName", nickName);
            parameters.Add("_userSeq", seq);

            await conn.ExecuteAsync("spUpdateUserInfo", parameters, trxn, commandType: CommandType.StoredProcedure);
        }
    }
}




