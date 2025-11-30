using Elastic.Clients.Elasticsearch;
using MassTransit;
using NewsPulse.Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<ElasticsearchClient>(sp =>
{
    // Docker'daki Elasticsearch adresi
    var settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"))
        .DefaultIndex("news-index"); // Varsayılan index adı

    return new ElasticsearchClient(settings);
});

// 2. MassTransit ve RabbitMQ Ayarları
builder.Services.AddMassTransit(x =>
{
    // Consumer'ı tanıt
    x.AddConsumer<NewsCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Kuyruk Ayarları (Endpoint)
        // "news-search-indexer" adında kalıcı bir kuyruk oluşacak.
        cfg.ReceiveEndpoint("news-search-indexer", e =>
        {
            // Bu kuyruğu hangi consumer dinleyecek?
            e.ConfigureConsumer<NewsCreatedConsumer>(context);
        });
    });
});

var host = builder.Build();
host.Run();
