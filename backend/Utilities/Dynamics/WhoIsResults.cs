using Bot.Services;
using Discord;

namespace Utilities.Dynamics;

public interface WhoIsResults
{
    public Task AddWhoIsInformation(EmbedBuilder embed, IGuildUser user, IInteractionContext context,
        Translation translator);
}