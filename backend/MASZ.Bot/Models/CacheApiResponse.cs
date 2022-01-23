namespace MASZ.Bot.Models;

public class CacheApiResponse
{
	public CacheApiResponse(object content, int cacheMinutes = 30)
	{
		Content = content;
		ExpiresAt = DateTime.Now.AddMinutes(cacheMinutes);
	}

	private object Content { get; }
	private DateTime ExpiresAt { get; }

	public T GetContent<T>()
	{
		return (T)Content;
	}

	public bool IsExpired()
	{
		return DateTime.Now > ExpiresAt;
	}
}