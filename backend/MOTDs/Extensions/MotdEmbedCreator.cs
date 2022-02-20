using Bot.Enums;
using Bot.Extensions;
using Bot.Services;
using Bot.Translators;
using Discord;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using MOTDs.Models;
using MOTDs.Translators;

namespace MOTDs.Extensions;

public static class MotdEmbedCreator
{
	public static async Task<EmbedBuilder> CreateMotdEmbed(this GuildMotd motd, IUser actor, RestAction action,
		IServiceProvider provider)
	{
		var translator = provider.GetRequiredService<Translation>();

		await translator.SetLanguage(motd.GuildId);

		var embed = await EmbedCreator.CreateBasicEmbed(action, provider, actor);

		embed.WithTitle(translator.Get<MotdTranslator>().MessageOfTheDay());

		switch (action)
		{
			case RestAction.Created:
				embed.WithDescription(translator.Get<MotdNotificationTranslator>().NotificationMotdInternalCreate(actor));
				break;
			case RestAction.Updated:
				embed.WithDescription(translator.Get<MotdNotificationTranslator>().NotificationMotdInternalEdited(actor));
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(action), action, null);
		}

		embed.AddField(translator.Get<MotdNotificationTranslator>().NotificationMotdShow(), motd.ShowMotd.GetCheckEmoji());
		embed.AddField(translator.Get<BotTranslator>().Message(), motd.Message.Truncate(1000));

		return embed;
	}
}