using Discord;
using Bot.Services;

namespace Utilities.Dynamics;

public interface WhoIsResults
{
	public Task AddWhoIsInformation(EmbedBuilder embed, IGuildUser user, IInteractionContext context,
		Translation translator);
}