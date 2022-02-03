using Discord;
using Humanizer;
using MASZ.Bot.Enums;
using MASZ.Bot.Extensions;
using MASZ.Bot.Services;
using MASZ.Bot.Translators;
using MASZ.MOTDs.Models;
using MASZ.MOTDs.Translators;
using Microsoft.Extensions.DependencyInjection;

namespace MASZ.MOTDs.Extensions;

public static class MotdEmbedCreator
{
	public static async Task<EmbedBuilder> CreateMotdEmbed(this GuildMotd motd, IUser actor, RestAction action,
		IServiceProvider provider)
	{
		var translator = provider.GetRequiredService<Translation>();

		await translator.SetLanguage(motd.GuildId);

		var embed = await EmbedCreator.CreateBasicEmbed(action, provider, actor);

		if (actor != null)
			embed.WithThumbnailUrl(actor.GetAvatarOrDefaultUrl());

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