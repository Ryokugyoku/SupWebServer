using Microsoft.EntityFrameworkCore;
using SupWebServer.DB;
using SupWebServer.System;
using SupWebServer.ViewModel;

namespace SupWebServer.BusinessModel.CategoryModule;

[Service] 
public class CategoryService : ICategoryService
{
    private readonly AppDbContext _dbContext;
    public CategoryService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<CategoryView>> GetAllCategorySync(string culture)
    {
        return await _dbContext.Categories
            .OrderBy(c => c.Id)
            .Select(c => new CategoryView
            {
                Id   = c.Id,
                Name = c.Translations
                           .Where(t => t.Culture == culture)
                           .Select(t => t.Name)
                           .FirstOrDefault()

                       ?? c.Translations
                           .Where(t => t.Culture == "en")
                           .Select(t => t.Name)
                           .FirstOrDefault()
            })
            .ToListAsync();

    }
}