using Discord;

namespace Bot.Extensions;

public static class UserAvatar
{
    public static string GetAvatarOrDefaultUrl(this IUser user, ImageFormat format = ImageFormat.Auto,
        ushort size = 1024) =>
        user.GetAvatarUrl(format, size) ?? user.GetDefaultAvatarUrl();
}
