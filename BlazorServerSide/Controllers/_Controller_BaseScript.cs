using Microsoft.AspNetCore.Mvc;

namespace BlazorServerSide.Controllers;

[ApiController]
[Route("api")]
public class _Controller_BaseScript : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = new LoginResponse
        {
            
        };

        return Ok(response);
    }

    private async Task<bool> AuthenticateUser(string userName, string password)
    {
        await Task.Delay(5000);
        if(userName == "Sy" && password == "123")
        {
            Console.WriteLine("Login successful");
            return true;
        }
        else
        {
            Console.WriteLine("Invalid username or password");
            return false;
        }
    }

    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        // You can add more properties as needed
    }
}