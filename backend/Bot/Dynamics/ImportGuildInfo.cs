using Bot.Models;

namespace Bot.Dynamics;

public interface IMportGuildInfo
{
    public Task ImportGuildInfo(GuildConfig guildConfig);
}
