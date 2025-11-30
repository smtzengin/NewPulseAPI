
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using NewsPulse.Application.Interfaces;
using NewsPulse.Domain.Entities;
using System.Text.Json;

namespace NewsPulse.Application.Features.News.Queries;

public record GetNewsByIdQuery(Guid Id) : IRequest<NewsArticle?>;

public class GetNewsByIdQueryHandler(
    IApplicationDbContext context,
    IDistributedCache cache) // <-- Redis Cache Inject Edildi
    : IRequestHandler<GetNewsByIdQuery, NewsArticle?>
{
    public async Task<NewsArticle?> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"news_{request.Id}";

        // 1. Önce Cache'e (Redis) Bak
        string? cachedNews = await cache.GetStringAsync(cacheKey, cancellationToken);
        if (!string.IsNullOrEmpty(cachedNews))
        {
            // Cache'de varsa deserialize et ve döndür (Veritabanına gitme!)
            return JsonSerializer.Deserialize<NewsArticle>(cachedNews);
        }

        // 2. Cache'de yoksa Veritabanına (SQL) Git
        var newsArticle = await context.NewsArticles
            .AsNoTracking() // Okuma performansı için
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        // 3. Veritabanında bulduysan Cache'e Yaz
        if (newsArticle != null)
        {
            var serializedNews = JsonSerializer.Serialize(newsArticle);

            await cache.SetStringAsync(cacheKey, serializedNews, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), // 10 dakika sakla
                SlidingExpiration = TimeSpan.FromMinutes(2) // 2 dk erişilmezse sil
            }, cancellationToken);
        }

        return newsArticle;
    }
}