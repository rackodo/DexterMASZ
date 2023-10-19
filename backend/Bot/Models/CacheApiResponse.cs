namespace Bot.Models;

public class CacheApiResponse(object content, int cacheMinutes = 30)
{
    private object Content { get; } = content;
    private DateTime ExpiresAt { get; } = DateTime.Now.AddMinutes(cacheMinutes);

    public T GetContent<T>() => (T)Content;

    public bool IsExpired() => DateTime.Now > ExpiresAt;
}
