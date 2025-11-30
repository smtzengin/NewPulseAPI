

using Microsoft.EntityFrameworkCore;
using NewsPulse.Domain.Entities;

namespace NewsPulse.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<NewsArticle> NewsArticles { get; }
}