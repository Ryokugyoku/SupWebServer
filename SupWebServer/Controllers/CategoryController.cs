using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupWebServer.DB.Tables;

namespace SupWebServer.Controllers;
/// <summary>
/// カテゴリーに関するAPI
/// </summary>
[ApiController]
[Route("/[controller]")]
public class CategoryController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("/list")]
    public IEnumerable<Category> Get()
    {
        return new List<Category>();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("/create")]
    public IActionResult Create(Category category)
    {
        return Ok();
    }


}