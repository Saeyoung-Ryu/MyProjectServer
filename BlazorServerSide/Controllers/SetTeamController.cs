using BlazorApp3.Common;
using Manager;
using Microsoft.AspNetCore.Mvc;
using Protocol;
using Type;

namespace BlazorServerSide.Controllers;

[ApiController]
[Route("api")]
public class SetTeamController : ControllerBase
{
    [HttpPost("MatchHistory/SetTeam")]
    public async Task<IActionResult> SetTeamAsync([FromBody] SetTeamReq request)
    {
        await LogDB.InsertLogMatchHistoryAsync(request.LogMatchHistory);
        await RankManager.SetUserWinAsync(request.LogMatchHistory.Team1Data);
        
        var response = new SetTeamRes
        {
            IsSuccess = true
        };
        
        return Ok(response);
    }
}