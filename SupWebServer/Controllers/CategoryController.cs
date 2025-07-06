using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupWebServer.BusinessModel;
using SupWebServer.DB.Tables;
using SupWebServer.ViewModel;

namespace SupWebServer.Controllers;
/// <summary>
/// カテゴリーに関するAPI
/// </summary>
[ApiController]
[Route("/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [AllowAnonymous]
    [HttpGet("/list")]
    public IEnumerable<CategoryView> Get()
    {
        var list = _categoryService.GetAllCategorySync("ja");
        list.Wait();
        return list.Result;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("/create")]
    public IActionResult Create(CreateCategoryView category)
    {
        return Ok();
    }

    
}