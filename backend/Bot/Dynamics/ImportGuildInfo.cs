using Bot.Models;

namespace Bot.Dynamics;

public interface ImportGuildInfo
{
    public Task ImportGuildInfo(GuildConfig guildConfig);
}