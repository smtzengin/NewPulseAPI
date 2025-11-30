

namespace NewsPulse.Domain.Events;

public record NewsCreatedEvent(
    Guid Id,
    string Title,
    string Content,
    string Category,
    DateTime CreatedAt
);