
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsPulse.Domain.Entities;

namespace NewsPulse.Infrastructure.Persistence.Configurations;

public class NewsArticleConfiguration : IEntityTypeConfiguration<NewsArticle>
{
    public void Configure(EntityTypeBuilder<NewsArticle> builder)
    {
        // Tablo Adı
        builder.ToTable("NewsArticles");

        // Primary Key
        builder.HasKey(x => x.Id);

        // Property Ayarları
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200); // SQL: NVARCHAR(200)

        builder.Property(x => x.Url)
            .IsRequired();

        // Indexleme: Arama ve performans için URL üzerinde index oluşturuyoruz.
        // Aynı haberin tekrar kaydedilmesini engellemek için Unique Index.
        builder.HasIndex(x => x.Url)
            .IsUnique();
    }
}
