using BlazorApp3.Common;
using Enum;
using Manager;
using Microsoft.AspNetCore.Mvc;
using Protocol;
using Type;

namespace BlazorServerSide.Controllers;

[ApiController]
[Route("api")]
public class WinRateHistoryController : ControllerBase
{
    [HttpPost("GetUserWinRateHistory")]
    public async Task<IActionResult> GetUserWinRateHistoryAsync([FromBody] GetUserWinRateHistoryReq request)
    {
        var userWinRateHistories = await AccountDB.GetUserWinRateHistoryAsync(request.Seq);
        
        var response = new GetUserWinRateHistoryRes
        {
            UserWinRateHistory = userWinRateHistories
        };

        return Ok(response);
    }
    
    [HttpPost("SetUserWinRateHistory")]
    public async Task<IActionResult> SetUserWinRateHistoryAsync([FromBody] SetUserWinRateHistoryReq request)
    {
        foreach (var item in request.UserWinRateHistory)
        {
            await AccountDB.SetUserWinRateHistoryAsync(item);
            
            /*if(item.WinCount > 0 || item.LoseCount > 0)
                await RankManager.SetUserWinLoseCount(request.NickName, request.Seq, item.WinCount, item.LoseCount, (LineType) item.LineType);*/
        }
        
        await RankManager.InitRankAsync();
        
        var response = new GetUserWinRateHistoryRes
        {
            
        };

        return Ok(response);
    }
}