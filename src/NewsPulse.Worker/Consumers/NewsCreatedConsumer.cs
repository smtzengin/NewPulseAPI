
using Elastic.Clients.Elasticsearch;
using MassTransit;
using NewsPulse.Domain.Events;

namespace NewsPulse.Worker.Consumers;

public class NewsCreatedConsumer(ElasticsearchClient elasticClient, ILogger<NewsCreatedConsumer> logger)
    : IConsumer<NewsCreatedEvent>
{
    public async Task Consume(ConsumeContext<NewsCreatedEvent> context)
    {
        var message = context.Message;

        logger.LogInformation($"📥 Yeni haber yakalandı: {message.Title}");

        //  Elasticsearch için dökümanı hazırla
        var newsDocument = new
        {
            Id = message.Id,
            Title = message.Title,
            Content = message.Content,
            Category = message.Category,
            CreatedAt = message.CreatedAt,
            SearchText = $"{message.Title} {message.Content} {message.Category}" // Full-text search için birleşik alan
        };

        // Elasticsearch'e İndeksle (Insert)
        // "news-index" adında bir index (tablo gibi düşün) oluşturur/kullanır.
        var response = await elasticClient.IndexAsync(newsDocument, idx => idx.Index("news-index"));

        if (response.IsValidResponse)
        {
            logger.LogInformation($"✅ Elasticsearch'e başarıyla kaydedildi! Index: {response.Index}"); 
        }
        else
        {
            logger.LogError($"❌ Elasticsearch hatası: {response.DebugInformation}");          
        }
    }
}