

using NewsPulse.Application.Interfaces.Repositories;
using NewsPulse.Domain.Entities;
using NewsPulse.Infrastructure.Persistence;

namespace NewsPulse.Infrastructure.Repositories;

public class NewsRepository(NewsPulseDbContext context) : INewsRepository
{
    public async Task<NewsArticle> AddAsync(NewsArticle article)
    {
        await context.NewsArticles.AddAsync(article);
        await context.SaveChangesAsync();
        return article;
    }
}
