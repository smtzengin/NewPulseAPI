
using NewsPulse.Domain.Entities;

namespace NewsPulse.Application.Interfaces.Repositories;

public interface INewsRepository
{
    Task<NewsArticle> AddAsync(NewsArticle article);
}
