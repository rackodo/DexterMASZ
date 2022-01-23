namespace MASZ.Bot.Extensions;

public static class GuildIcon
{
	public static string GetAnimatedOrDefaultAvatar(this string iconUrl)
	{
		if (iconUrl == null) return null;

		iconUrl = iconUrl.Replace(".jpg", iconUrl.Split("/").Last().StartsWith("a_") ? ".gif" : ".png");

		if (!iconUrl.Search('?'))
			iconUrl += "?size=512";

		return iconUrl;
	}
}