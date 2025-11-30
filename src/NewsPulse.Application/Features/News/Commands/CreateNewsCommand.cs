
using MassTransit;
using MediatR;
using NewsPulse.Application.Interfaces.Repositories;
using NewsPulse.Domain.Entities;
using NewsPulse.Domain.Events;

namespace NewsPulse.Application.Features.News.Commands;

public record CreateNewsCommand(
    string Title,
    string Content,
    string Author,
    string Url,
    string Source,
    string Category
) : IRequest<Guid>;

public class CreateNewsCommandHandler(INewsRepository newsRepository, IPublishEndpoint publishEndpoint)
    : IRequestHandler<CreateNewsCommand, Guid>
{
    public async Task<Guid> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
    {
        var newsArticle = new NewsArticle(
            request.Title,
            request.Content,
            request.Author,
            request.Url,
            request.Source,
            request.Category
        );

        await newsRepository.AddAsync(newsArticle);

        await publishEndpoint.Publish(new NewsCreatedEvent(
            newsArticle.Id,
            newsArticle.Title,
            newsArticle.Content,
            newsArticle.Category,
            newsArticle.PublishedDate
        ), cancellationToken);

        return newsArticle.Id;
    }
}