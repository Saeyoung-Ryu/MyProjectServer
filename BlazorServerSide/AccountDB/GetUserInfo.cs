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
        public static async Task<UserInfo> GetUserInfoAsync(string nickName)
        {
            await using (var conn = new MySqlConnection(MyProjectInfoConfig.Instance.ConnectionString))
            {
                return await SpGetUserInfoAsync(conn, null, nickName);
            }
        }

        private static async Task<UserInfo> SpGetUserInfoAsync(MySqlConnection conn, MySqlTransaction trxn, string nickName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("_userName", nickName);

            return await conn.QuerySingleOrDefaultAsync<UserInfo>("spGetUserInfo", parameters, trxn, commandType: CommandType.StoredProcedure);
        }

        public static async Task<UserInfo> GetUserInfoWithIdAsync(int seq)
        {
            await using (var conn = new MySqlConnection(MyProjectInfoConfig.Instance.ConnectionString))
            {
                return await conn.QuerySingleOrDefaultAsync<UserInfo>($"select * from tblUserInfo where seq = {seq}");
            }
        }
    }
}




