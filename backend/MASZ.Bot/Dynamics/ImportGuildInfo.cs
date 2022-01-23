using MASZ.Bot.Models;

namespace MASZ.Bot.Dynamics;

public interface ImportGuildInfo
{
	public Task ImportGuildInfo(GuildConfig guildConfig);
}