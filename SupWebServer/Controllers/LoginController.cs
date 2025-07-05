namespace SupWebServer.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]  
public class LoginController:ControllerBase 

{
    [HttpPost]
    public IActionResult Post([FromBody] LoginRequest request)
    {
        // 本来は DB などで照合する
        if (request.UserName == "admin" && request.Password == "P@ssw0rd")
        {
            var result = new LoginResponse
            {
                Success = true,
                Token   = "sample-jwt-token"
            };
            return Ok(result);                  // 200 OK + JSON
        }

        return Unauthorized(new LoginResponse   // 401
        {
            Success = false,
            Message = "ユーザー名またはパスワードが間違っています。"
        });
    }

}

public record LoginRequest
{
    public string UserName { get; init; } = default!;
    public string Password { get; init; } = default!;
}

public record LoginResponse
{
    public bool   Success { get; init; }
    public string? Token   { get; init; }
    public string? Message { get; init; }
}
