using Bot.Services;
using Discord;

namespace Utilities.Dynamics;

public interface IWhoIsResults
{
    public Task AddWhoIsInformation(EmbedBuilder embed, IGuildUser user, IInteractionContext context,
        Translation translator);
}
