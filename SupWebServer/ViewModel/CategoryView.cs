namespace SupWebServer.ViewModel;

/// <summary>
/// カテゴリー一覧を返す際に利用されているビュー 
/// </summary>
public class CategoryView
{
    public int    Id   { get; set; }
    public string? Name { get; set; } = null!;

}