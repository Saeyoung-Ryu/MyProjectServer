using System.Data;
using BlazorApp3.Common;
using Common;
using Dapper;
using MySqlConnector;

namespace BlazorApp3.Common
{
    public partial class AccountDB
    {
        public static async Task SetUserInfoAsync(string nickName, string? linkedMail = null)
        {
            await using (var conn = new MySqlConnection(MyProjectInfoConfig.Instance.ConnectionString))
            {
                await SpSetUserInfoAsync(conn, null, nickName, linkedMail);
            }
        }

        private static async Task SpSetUserInfoAsync(MySqlConnection conn, MySqlTransaction trxn, string nickName, string? linkedMail)
        {
            var parameters = new DynamicParameters();
            parameters.Add("_userName", nickName);
            parameters.Add("_linkedMail", linkedMail);

            await conn.ExecuteAsync("spInsertUserInfo", parameters, trxn, commandType: CommandType.StoredProcedure);
        }
    }
}




