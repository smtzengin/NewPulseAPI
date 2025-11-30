

using Bogus;
using NewsPulse.Domain.Entities;

namespace NewsPulse.Infrastructure.Persistence;

public static class NewsContextSeed
{
    public static async Task SeedAsync(NewsPulseDbContext context)
    {
        // Eğer veritabanında zaten veri varsa hiç dokunma, çık.
        if (context.NewsArticles.Any())
        {
            return;
        }

        // --- BOGUS İLE VERİ ÜRETİMİ ---

        var newsFaker = new Faker<NewsArticle>("tr")
            .CustomInstantiator(f => new NewsArticle(
                // Constructor parametrelerini rastgele dolduruyoruz:

                title: f.Lorem.Sentence(5, 5),        // 5-10 kelimelik başlık
                content: f.Lorem.Paragraphs(3),       // 3 paragraflık içerik
                author: f.Name.FullName(),            // Ad Soyad (Ali Yılmaz gibi)
                url: f.Internet.Url(),                // Rastgele URL
                source: f.PickRandom(new[] { "CNN Türk", "BBC", "Anadolu Ajansı", "Webrazzi", "ShiftDelete" }), // Kaynaklar
                category: f.PickRandom(new[] { "Teknoloji", "Ekonomi", "Spor", "Sağlık", "Yapay Zeka" })       // Kategoriler
            ));

        // 200 Adet Haber Üret
        var newsList = newsFaker.Generate(200);

        // Veritabanına Ekle
        await context.NewsArticles.AddRangeAsync(newsList);
        await context.SaveChangesAsync();
    }
}
