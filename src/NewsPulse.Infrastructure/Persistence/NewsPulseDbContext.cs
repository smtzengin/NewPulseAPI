using Microsoft.EntityFrameworkCore;
using NewsPulse.Domain.Entities;

namespace NewsPulse.Infrastructure.Persistence;

public class NewsPulseDbContext : DbContext
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
