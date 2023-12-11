using BlazorApp3.Common;
using Manager;
using Microsoft.AspNetCore.Mvc;
using Protocol;
using Type;

namespace BlazorServerSide.Controllers;

[ApiController]
[Route("api")]
public class RankingController : ControllerBase
{
    [HttpPost("Ranking/GetRanking")]
    public IActionResult GetRanking([FromBody] GetRankingReq request)
    {
        Console.WriteLine("GetRanking");
        var response = new GetRankingRes()
        {
            TotalRankList = RankManager.totalRankList,
            AdcRankList = RankManager.adcRankList,
            SupRankList = RankManager.supRankList,
            MidRankList = RankManager.midRankList,
            JgRankList = RankManager.jgRankList,
            TopRankList = RankManager.topRankList
        };

        return Ok(response);
    }

    [HttpPost("Ranking/ResetRanking")]
    public async Task<IActionResult> ResetRankingAsync([FromBody] ResetRankingReq request)
    {
        DateTime time = DateTime.UtcNow;

        int userRank = 1;

        foreach (var rankInfo in RankManager.totalRankList)
        {
            RankHistory rankHistory = new RankHistory()
            {
                UserSeq = rankInfo.Seq,
                Time = time,
                Ranking = userRank,
                WinRate = rankInfo.WinRate
            };

            userRank++;
            await AccountDB.SetRankHistoryAsync(rankHistory);
        }

        RankManager.jgRankList.Clear();
        RankManager.midRankList.Clear();
        RankManager.topRankList.Clear();
        RankManager.adcRankList.Clear();
        RankManager.supRankList.Clear();
        RankManager.totalRankList.Clear();
        await AccountDB.ResetRankAsync();
        
        var response = new ResetRankingRes()
        {
            TotalRankList = RankManager.totalRankList,
            AdcRankList = RankManager.adcRankList,
            SupRankList = RankManager.supRankList,
            MidRankList = RankManager.midRankList,
            JgRankList = RankManager.jgRankList,
            TopRankList = RankManager.topRankList
        };

        return Ok(response);
    }
}