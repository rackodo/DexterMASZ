using MASZ.AutoMods.Views;
using MASZ.Bot.Extensions;

namespace MASZ.AutoMods.Extensions;

public static class SearchContains
{
	public static bool Search(this string search, AutoModEventExpandedView obj)
	{
		if (obj == null)
			return false;

		return search.Search(obj.AutoModerationEvent) ||
		       search.Search(obj.Suspect);
	}

	public static bool Search(this string search, AutoModEventView obj)
	{
		if (obj == null)
			return false;

		return search.Search(obj.AutoModerationAction.ToString()) ||
		       search.Search(obj.AutoModerationType.ToString()) ||
		       search.Search(obj.CreatedAt) ||
		       search.Search(obj.Discriminator) ||
		       search.Search(obj.Username) ||
		       search.Search(obj.Nickname) ||
		       search.Search(obj.UserId) ||
		       search.Search(obj.MessageContent) ||
		       search.Search(obj.MessageId);
	}
}