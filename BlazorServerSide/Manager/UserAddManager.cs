using System;
using BlazorApp3.Common;
using Enum;
using Type;

namespace Manager
{
    public class UserAddManager
    {
        public static async Task SetNewUserAsync(string nickname, string? linkedMail = null)
        {
            await AccountDB.SetUserInfoAsync(nickname, linkedMail);
            var userSeq = await AccountDB.GetUserInfoApproximateAsync(nickname);

            foreach (LineType lineType in System.Enum.GetValues(typeof(LineType)))
            {
                if(lineType == LineType.Random || lineType == LineType.None)
                    continue;
                
                UserWinRateHistory userWinRateHistory = new UserWinRateHistory()
                {
                    UserSeq = userSeq.Seq,
                    LineType = (int) lineType,
                    WinCount = 0,
                    LoseCount = 0
                };
                
                await AccountDB.SetUserWinRateHistoryAsync(userWinRateHistory);
            }
        }
    }
}
