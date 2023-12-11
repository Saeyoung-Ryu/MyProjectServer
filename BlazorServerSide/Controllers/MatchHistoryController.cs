using BlazorApp3.Common;
using Manager;
using Microsoft.AspNetCore.Mvc;
using Protocol;

namespace BlazorServerSide.Controllers;

[ApiController]
[Route("api")]
public class MatchHistoryController : ControllerBase
{
    [HttpPost("GetMatchHistory")]
    public async Task<IActionResult> GetMatchHistoryAsync([FromBody] GetMatchHistoryReq request)
    {
        var matchHistory = await LogDB.GetLogMatchHistoryAsync();
        var response = new GetMatchHistoryRes()
        {
            LogMatchHistoryList = matchHistory
        };

        return Ok(response);
    }
    
    [HttpPost("SetTeamWin")]
    public async Task<IActionResult> SetTeamWin([FromBody] SetTeamWinReq request)
    {
        if (request.WinTeam == 1)
        {
            request.LogMatchHistory.Team1Win = 1;
            await LogDB.SetLogMatchHistoryAsync(request.LogMatchHistory);

            await RankManager.SetUserWinAsync(request.LogMatchHistory.Team1Data);
            await RankManager.SetUserLoseAsync(request.LogMatchHistory.Team2Data);
        }
        else if (request.WinTeam == 2)
        {
            request.LogMatchHistory.Team2Win = 1;
            await LogDB.SetLogMatchHistoryAsync(request.LogMatchHistory);

            await RankManager.SetUserWinAsync(request.LogMatchHistory.Team2Data);
            await RankManager.SetUserLoseAsync(request.LogMatchHistory.Team1Data);
        }
        
        var response = new SetTeamWinRes()
        {
            
        };

        return Ok(response);
    }
}