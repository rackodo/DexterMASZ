using MASZ.AutoMods.Models;
using MASZ.Bot.Extensions;

namespace MASZ.AutoMods.Extensions;

public static class SearchContains
{
	public static bool Search(this string search, AutoModEventExpanded obj)
	{
		if (obj == null)
			return false;

		return search.Search(obj.AutoModerationEvent) ||
			   search.Search(obj.Suspect);
	}

	public static bool Search(this string search, AutoModEvent obj)
	{
		if (obj == null)
			return false;

		return search.Search(obj.AutoModAction.ToString()) ||
			   search.Search(obj.AutoModType.ToString()) ||
			   search.Search(obj.CreatedAt) ||
			   search.Search(obj.Discriminator) ||
			   search.Search(obj.Username) ||
			   search.Search(obj.Nickname) ||
			   search.Search(obj.UserId) ||
			   search.Search(obj.MessageContent) ||
			   search.Search(obj.MessageId);
	}
}