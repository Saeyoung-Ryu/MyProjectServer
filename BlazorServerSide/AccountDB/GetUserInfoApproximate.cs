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
        public static async Task<UserInfo> GetUserInfoApproximateAsync(string nickName)
        {
            await using (var conn = new MySqlConnection(MyProjectInfoConfig.Instance.ConnectionString))
            {
                return await SpGetUserInfoApproximateAsync(conn, null, nickName);
            }
        }

        private static async Task<UserInfo> SpGetUserInfoApproximateAsync(MySqlConnection conn, MySqlTransaction trxn, string nickName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("_userName", nickName);

            return await conn.QuerySingleOrDefaultAsync<UserInfo>("spGetUserInfoApproximate", parameters, trxn, commandType: CommandType.StoredProcedure);
        }
    }
}




