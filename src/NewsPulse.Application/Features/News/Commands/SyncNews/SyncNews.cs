using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed; 
using NewsPulse.Application.Interfaces;
using NewsPulse.Domain.Events;
using System.Text.Json; 

namespace NewsPulse.Application.Features.News.Commands.SyncNews;

public record SyncNewsCommand() : IRequest<Unit>;

public class SyncNewsCommandHandler(
    IApplicationDbContext context,
    IPublishEndpoint publishEndpoint,
    IDistributedCache cache) 
    : IRequestHandler<SyncNewsCommand, Unit>
{
    public async Task<Unit> Handle(SyncNewsCommand request, CancellationToken cancellationToken)
    {
        var allNews = await context.NewsArticles
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        foreach (var news in allNews)
        {

            await publishEndpoint.Publish(new NewsCreatedEvent(
                news.Id,
                news.Title,
                news.Content,
                news.Category,
                news.CreatedAt ?? DateTime.UtcNow
            ), cancellationToken);

            string cacheKey = $"news_{news.Id}";
            string serializedNews = JsonSerializer.Serialize(news);

            await cache.SetStringAsync(cacheKey, serializedNews, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) 
            }, cancellationToken);
        }

        return default;
    }
}