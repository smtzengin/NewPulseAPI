using Microsoft.EntityFrameworkCore;
using NewsPulse.Domain.Entities;
using NewsPulse.Application.Interfaces;

namespace NewsPulse.Infrastructure.Persistence;

public class NewsPulseDbContext : DbContext,IApplicationDbContext
{

    public NewsPulseDbContext(DbContextOptions<NewsPulseDbContext> options) : base(options)
    {
    }

    public DbSet<NewsArticle> NewsArticles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NewsPulseDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
