using BlazorApp3.Common;
using Manager;
using Microsoft.AspNetCore.Mvc;
using Protocol;

namespace BlazorServerSide.Controllers;

[ApiController]
[Route("api")]
public class UserController : ControllerBase
{
    [HttpPost("GetUserInfo")]
    public async Task<IActionResult> GetUserInfoAsync([FromBody] GetUserReq request)
    {
        var user = await AccountDB.GetUserInfoAsync(request.NickName);
        
        var response = new GetUserRes()
        {
            UserInfo = user
        };

        return Ok(response);
    }
    
    [HttpPost("GetUserInfoApproximate")]
    public async Task<IActionResult> GetUserInfoApproximateAsync([FromBody] GetUserReq request)
    {
        var user = await AccountDB.GetUserInfoApproximateAsync(request.NickName);
        
        var response = new GetUserRes()
        {
            UserInfo = user
        };

        return Ok(response);
    }
    
    [HttpPost("SetNewUser")]
    public async Task<IActionResult> SetNewUserAsync([FromBody] SetNewUserReq request)
    {
        try
        {
            await UserAddManager.SetNewUserAsync(request.NickName);
        
            var response = new SetNewUserRes()
            {
                IsSuccess = true
            };

            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            var response = new SetNewUserRes()
            {
                IsSuccess = false
            };

            return Ok(response);
        }
    }
    
    [HttpPost("SetUserNickName")]
    public async Task<IActionResult> SetUserNickNameAsync([FromBody] SetUserNickNameReq request)
    {
        bool isDuplicatedNickName = true;
        
        var user = await AccountDB.GetUserInfoAsync(request.NickName);

        if (user == null)
        {
            await AccountDB.UpdateUserInfoAsync(request.NickName, request.Seq);
            isDuplicatedNickName = false;
        }
        
        var response = new SetUserNickNameRes()
        {
            IsDuplicatedNickName = isDuplicatedNickName
        };

        return Ok(response);
    }
}