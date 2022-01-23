using MASZ.Punishments.Models;

namespace MASZ.Punishments.Views;

public class CommentsView
{
	public CommentsView(ModCaseComment comment)
	{
		Id = comment.Id;
		Message = comment.Message;
		CreatedAt = comment.CreatedAt;
		UserId = comment.UserId.ToString();
	}

	public int Id { get; set; }
	public string Message { get; set; }
	public DateTime CreatedAt { get; set; }
	public string UserId { get; set; }
}