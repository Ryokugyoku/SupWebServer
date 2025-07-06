using SupWebServer.ViewModel;

namespace SupWebServer.BusinessModel;

public interface ICategoryService
{
    Task<List<CategoryView>>GetAllCategorySync(string culture);
}