using Discord;
using MASZ.Bot.Services;

namespace MASZ.Utilities.Dynamics;

public interface WhoIsResults
{
	public Task AddWhoIsInformation(EmbedBuilder embed, IGuildUser user, IInteractionContext context,
		Translation translator);
}