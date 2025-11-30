

namespace NewsPulse.Domain.Entities;

public class NewsArticle : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public string Url { get; private set; } = string.Empty;
    public string Source { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty; 

    public DateTime PublishedDate { get; private set; }

    public NewsArticle(string title, string content, string author, string url, string source, string category)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException(nameof(title));
        if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

        Title = title;
        Content = content;
        Author = author;
        Url = url;
        Source = source;
        Category = category;

        // Tarihi dışarıdan sormuyoruz, oluşturulduğu anı basıyoruz.
        PublishedDate = DateTime.UtcNow;
    }

    public void UpdateContent(string newContent)
    {
        if (string.IsNullOrWhiteSpace(newContent)) throw new ArgumentException("İçerik boş olamaz.");
        Content = newContent;
        UpdatedAt = DateTime.UtcNow;
    }
}
