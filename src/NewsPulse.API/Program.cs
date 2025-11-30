using MassTransit;
using Microsoft.EntityFrameworkCore;
using NewsPulse.Application.Features.News.Commands;
using NewsPulse.Application.Interfaces;
using NewsPulse.Application.Interfaces.Repositories;
using NewsPulse.Infrastructure.Persistence;
using NewsPulse.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NewsPulseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<NewsPulseDbContext>());

builder.Services.AddScoped<INewsRepository, NewsRepository>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateNewsCommand).Assembly));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "NewsPulse_"; // Key'lerin başına bu eklenir
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<NewsPulseDbContext>();

        // Veritabanı yoksa oluştur (Migration'ı uygula)
        context.Database.EnsureCreated();

        // Verileri bas
        await NewsContextSeed.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanı seed edilirken bir hata oluştu.");
    }
}

app.Run();

