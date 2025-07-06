using System.ComponentModel.DataAnnotations.Schema;

namespace SupWebServer.DB.Tables;

public class Category
{
    public int Id { get; set; }
    public ICollection<CategoryTranslation> Translations { get; set; } = [];

    // 便利プロパティ: 現在カルチャの翻訳 (存在しなければ既定カルチャ)
    [NotMapped]
    public CategoryTranslation Current =>
        Translations.FirstOrDefault(t => t.Culture == CurrentCulture())
        ?? Translations.First(t => t.Culture == DefaultCulture);

    private static string CurrentCulture() =>
        Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

    private const string DefaultCulture = "en";
}

public class CategoryTranslation
{
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public string Culture { get; set; } = null!;          // "en", "ja" …
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
}
