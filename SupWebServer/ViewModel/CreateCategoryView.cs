namespace SupWebServer.ViewModel;

public class CreateCategoryView
{
    /// <summary>
    /// 親が存在しない場合はNullでも問題ない
    /// </summary>
    public int ParentId { get; set; }
    /// <summary>
    /// 必須項目 言語が設定される。
    /// </summary>
    public string Culture { get; set; } = null!;
    /// <summary>
    /// カテゴリー名
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// カテゴリー詳細　長文
    /// </summary>
    public string Description { get; set; } = null!;
    /// <summary>
    /// カテゴリー詳細　短文
    /// </summary>
    public string ShortDescription { get; set; } = null!;
}