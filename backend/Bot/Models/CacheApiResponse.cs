namespace Bot.Models;

public class CacheApiResponse
{
    private object Content { get; }
    private DateTime ExpiresAt { get; }

    public CacheApiResponse(object content, int cacheMinutes = 30)
    {
        Content = content;
        ExpiresAt = DateTime.Now.AddMinutes(cacheMinutes);
    }

    public T GetContent<T>() => (T)Content;

    public bool IsExpired() => DateTime.Now > ExpiresAt;
}