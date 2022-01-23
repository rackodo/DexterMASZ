using MASZ.UserNotes.Models;

namespace MASZ.UserNotes.Views;

public class UserNoteView
{
	public UserNoteView(UserNote userNote)
	{
		Id = userNote.Id;
		GuildId = userNote.GuildId.ToString();
		UserId = userNote.UserId.ToString();
		Description = userNote.Description;
		CreatorId = userNote.CreatorId.ToString();
		UpdatedAt = userNote.UpdatedAt;
	}

	public int Id { get; set; }
	public string GuildId { get; set; }
	public string UserId { get; set; }
	public string Description { get; set; }
	public string CreatorId { get; set; }
	public DateTime UpdatedAt { get; set; }
}