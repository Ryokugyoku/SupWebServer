using Microsoft.EntityFrameworkCore;
using SupWebServer.DB.Tables;

namespace SupWebServer.DB;

/// <summary>
/// アプリケーション用 DbContext
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // --- DbSet -------------------------------------------------------------
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<CategoryTranslation> CategoryTranslations => Set<CategoryTranslation>();

    // --- Fluent API 設定 ----------------------------------------------------
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // PostgreSQL では既定で Unicode なので IsUnicode(false) を外しても良い
        modelBuilder.Entity<CategoryTranslation>(b =>
        {
            b.HasKey(t => new { t.CategoryId, t.Culture });

            // varchar(5) として明示
            b.Property(t => t.Culture)
                .HasColumnType("varchar(5)");

            b.HasOne(t => t.Category)
                .WithMany(c => c.Translations)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>()
            .Property(c => c.Id)
            .UseIdentityAlwaysColumn();          // ← PostgreSQL の IDENTITY
    }

}