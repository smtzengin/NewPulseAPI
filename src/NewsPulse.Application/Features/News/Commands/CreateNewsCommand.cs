

using MediatR;
using NewsPulse.Application.Interfaces.Repositories;
using NewsPulse.Domain.Entities;

namespace NewsPulse.Application.Features.News.Commands;

public record CreateNewsCommand(
    string Title,
    string Content,
    string Author,
    string Url,
    string Source,
    string Category
) : IRequest<Guid>;

public class CreateNewsCommandHandler(INewsRepository newsRepository)
    : IRequestHandler<CreateNewsCommand, Guid>
{
    public async Task<Guid> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
    {
        // 1. Domain Entity oluştur (Constructor içindeki validasyonlar çalışır)
        var newsArticle = new NewsArticle(
            request.Title,
            request.Content,
            request.Author,
            request.Url,
            request.Source,
            request.Category
        );

        // 2. Veritabanına kaydet
        await newsRepository.AddAsync(newsArticle);

        // *** KRİTİK NOKTA ***
        // İleride buraya "RabbitMQ'ya event fırlat" kodunu ekleyeceğiz.
        // Şimdilik sadece SQL'e yazıp ID dönüyoruz.

        return newsArticle.Id;
    }
}