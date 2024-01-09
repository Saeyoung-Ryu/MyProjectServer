using System;
using System.Text.RegularExpressions;
using BlazorApp3.Common;
using Enum;
using Type;

namespace Manager
{
    public class RankManager
    {
        public static List<RankInfo> totalRankList = new List<RankInfo>();
        
        public static List<RankInfo> adcRankList = new List<RankInfo>();
        public static List<RankInfo> supRankList = new List<RankInfo>();
        public static List<RankInfo> midRankList = new List<RankInfo>();
        public static List<RankInfo> jgRankList = new List<RankInfo>();
        public static List<RankInfo> topRankList = new List<RankInfo>();

        public static async Task InitRankAsync()
        {
            totalRankList = new List<RankInfo>();
            adcRankList = new List<RankInfo>();
            supRankList = new List<RankInfo>();
            midRankList = new List<RankInfo>();
            jgRankList = new List<RankInfo>();
            topRankList = new List<RankInfo>();
        
            await SetTotalRankInfoListAsync();
            await SetOtherLaneRanks();
        }
        private static async Task SetTotalRankInfoListAsync()
        {
        var infos = await AccountDB.GetAllUserWinRateHistory();

        List<List<UserWinRateHistory>> groupedInfo = infos
            .GroupBy(info => info.UserSeq)
            .Select(group => group.ToList())
            .ToList();

        int rankOrder = 1;
        
        foreach (var grouped in groupedInfo)
        {
            RankInfo rankInfo = new RankInfo();
            rankInfo.NickName = String.Empty;
            rankInfo.Rank = rankOrder;

            foreach (var item in grouped)
            {
                if (rankInfo.NickName == String.Empty)
                {
                    rankInfo.NickName = await item.GetNickNameAsync();
                    if (rankInfo.NickName == String.Empty)
                        break;
                }
                
                rankInfo.Seq = item.UserSeq;
                rankInfo.WinCount += item.WinCount;
                rankInfo.LoseCount += item.LoseCount;
                rankOrder++;
            }

            if(rankInfo.NickName == String.Empty)
                continue;

            if (rankInfo.WinCount + rankInfo.LoseCount != 0)
            {
                double value = (double) rankInfo.WinCount / (rankInfo.WinCount + rankInfo.LoseCount) * 100;
                rankInfo.WinRate  = Math.Round(value, 1);
                rankInfo.OverallScore = rankInfo.WinCount * 12 - rankInfo.LoseCount * 10;
                totalRankList.Add(rankInfo);
            }
        }

        totalRankList = totalRankList.OrderByDescending(e => e.OverallScore).ToList();
    }
        private static async Task SetOtherLaneRanks()
    {
        var supList = (await AccountDB.GetAllUserWinRateHistoryByLine(LineType.Support)).OrderByDescending(e => e.GetOverAllScore()).ToList();
        var adcList = (await AccountDB.GetAllUserWinRateHistoryByLine(LineType.Adc)).OrderByDescending(e => e.GetOverAllScore()).ToList();
        var midList = (await AccountDB.GetAllUserWinRateHistoryByLine(LineType.Mid)).OrderByDescending(e => e.GetOverAllScore()).ToList();
        var jgList = (await AccountDB.GetAllUserWinRateHistoryByLine(LineType.Jungle)).OrderByDescending(e => e.GetOverAllScore()).ToList();
        var topList = (await AccountDB.GetAllUserWinRateHistoryByLine(LineType.Top)).OrderByDescending(e => e.GetOverAllScore()).ToList();

        supRankList = await SetListAsync(supList);
        adcRankList = await SetListAsync(adcList);
        midRankList = await SetListAsync(midList);
        jgRankList = await SetListAsync(jgList);
        topRankList = await SetListAsync(topList);
    }
        private static async Task<List<RankInfo>> SetListAsync(List<UserWinRateHistory> userWinRateList)
        {
            List<RankInfo> list = new List<RankInfo>();
    
            int rankOrder = 1;
    
            foreach (var sup in userWinRateList)
            {
                RankInfo rankInfo = new RankInfo();
    
                rankInfo.NickName = await sup.GetNickNameAsync();
                if (rankInfo.NickName == String.Empty)
                    continue;
    
                rankInfo.Rank = rankOrder;
                rankInfo.WinCount = sup.WinCount;
                rankInfo.LoseCount = sup.LoseCount;
                rankInfo.OverallScore = sup.GetOverAllScore();
                rankInfo.WinRate = sup.GetWinRate();
    
                if (rankInfo.WinRate < 0)
                    continue;
    
                list.Add(rankInfo);
                rankOrder++;
            }
    
            return list;
        }
        public static async Task SetUserWinAsync(string teamData)
        {
            teamData = Regex.Replace(teamData, @",\s+", ",");
            string[] nickNameArray = teamData.Split(new char[] {','});
            
            if(nickNameArray.Length != 5)
                return;

            for (int i = 0; i < nickNameArray.Length; i++)
            {
                var user = await AccountDB.GetUserInfoApproximateAsync(nickNameArray[i]);

                if (i == 0)
                    await SetUserWinLose(user.Seq, WinLoseType.Win, LineType.Top);
                else if (i == 1)
                    await SetUserWinLose(user.Seq, WinLoseType.Win, LineType.Jungle);
                else if (i == 2)
                    await SetUserWinLose(user.Seq, WinLoseType.Win, LineType.Mid);
                else if (i == 3)
                    await SetUserWinLose(user.Seq, WinLoseType.Win, LineType.Adc);
                else if (i == 4)
                    await SetUserWinLose(user.Seq, WinLoseType.Win, LineType.Support);
            }
        }
        public static async Task SetUserLoseAsync(string teamData)
        {
            teamData = Regex.Replace(teamData, @",\s+", ",");
            string[] nickNameArray = teamData.Split(new char[] {','});
            
            if(nickNameArray.Length != 5)
                return;

            for (int i = 0; i < nickNameArray.Length; i++)
            {
                var user = await AccountDB.GetUserInfoApproximateAsync(nickNameArray[i]);

                if (i == 0)
                    await SetUserWinLose(user.Seq, WinLoseType.Lose, LineType.Top);
                else if (i == 1)
                    await SetUserWinLose(user.Seq, WinLoseType.Lose, LineType.Jungle);
                else if (i == 2)
                    await SetUserWinLose(user.Seq, WinLoseType.Lose, LineType.Mid);
                else if (i == 3)
                    await SetUserWinLose(user.Seq, WinLoseType.Lose, LineType.Adc);
                else if (i == 4)
                    await SetUserWinLose(user.Seq, WinLoseType.Lose, LineType.Support);
            }
        }
        private static void SetWrListInMemory(int seq, WinLoseType winLoseType)
        {
            foreach (var rankInfo in totalRankList)
            {
                if (rankInfo.Seq == seq)
                {
                    if (winLoseType == WinLoseType.Win)
                        rankInfo.WinCount++;
                    else if (winLoseType == WinLoseType.Lose)
                        rankInfo.LoseCount++;
                    
                    return;
                }
            }
        }
        private static async Task SetUserWinLose(int seq, WinLoseType winLoseType, LineType lineType)
        {
            var userWinRateHistoryList = await AccountDB.GetUserWinRateHistoryAsync(seq);
            var userWinRateHistory = userWinRateHistoryList.FirstOrDefault(e => e.LineType == (int) lineType);
                    
            if(userWinRateHistory == null)
                return;
                    
            SetWrListInMemory(seq, winLoseType);
                    
            if(winLoseType == WinLoseType.Win)
                userWinRateHistory.WinCount++;
            else if(winLoseType == WinLoseType.Lose)
                userWinRateHistory.LoseCount++;
            
            await AccountDB.SetUserWinRateHistoryAsync(userWinRateHistory);
        }
        public static async Task SetUserWinLoseCount(string nickName, int seq, int winCount, int loseCount, LineType lineType)
        {
            List<RankInfo> rankInfos = new List<RankInfo>();
            
            if(lineType == LineType.Adc)
                rankInfos = adcRankList;
            else if(lineType == LineType.Support)
                rankInfos = supRankList;
            else if(lineType == LineType.Mid)
                rankInfos = midRankList;
            else if(lineType == LineType.Jungle)
                rankInfos = jgRankList;
            else if(lineType == LineType.Top)
                rankInfos = topRankList;

            if (rankInfos.Any(e => e.Seq == seq) == false)
            {
                rankInfos.Add(new RankInfo()
                {
                    NickName = nickName,
                    Seq = seq,
                    WinCount = 0,
                    LoseCount = 0,
                    WinRate = 0,
                    Rank = 0,
                });
            }
            
            foreach (var rankInfo in rankInfos)
            {
                if (rankInfo.Seq == seq)
                {
                    rankInfo.WinCount = winCount;
                    rankInfo.LoseCount = loseCount;
                    break;
                }
            }

            foreach (var rankInfo in totalRankList)
            {
                if (rankInfo.Seq == seq)
                {
                    rankInfo.WinCount = adcRankList.First(e => e.Seq == seq).WinCount +
                                        supRankList.First(e => e.Seq == seq).WinCount +
                                        midRankList.First(e => e.Seq == seq).WinCount +
                                        jgRankList.First(e => e.Seq == seq).WinCount +
                                        topRankList.First(e => e.Seq == seq).WinCount;
                    
                    return;
                }
            }
        }
    }
}
