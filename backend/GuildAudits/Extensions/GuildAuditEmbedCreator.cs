using Bot.Enums;
using Bot.Extensions;
using Bot.Services;
using Bot.Translators;
using Discord;
using GuildAudits.Models;
using GuildAudits.Translators;
using Microsoft.Extensions.DependencyInjection;

namespace GuildAudits.Extensions;

public static class GuildAuditEmbedCreator
{
	public static async Task<EmbedBuilder> CreateGuildAuditEmbed(this GuildAuditConfig config, IUser actor,
		RestAction action, IServiceProvider provider)
	{
		var translator = provider.GetRequiredService<Translation>();

		await translator.SetLanguage(config.GuildId);

		var embed = await EmbedCreator.CreateBasicEmbed(action, provider, actor);

		if (actor != null)
			embed.WithThumbnailUrl(actor.GetAvatarOrDefaultUrl());

		embed.WithTitle(translator.Get<GuildAuditNotificationTranslator>().NotificationGuildAuditTitle() + ": " +
				translator.Get<GuildAuditEnumTranslator>().Enum(config.GuildAuditEvent));

		var guildEventTypeName =
			$"**{translator.Get<GuildAuditEnumTranslator>().Enum(config.GuildAuditEvent).ToLower()}**";

		switch (action)
		{
			case RestAction.Created:
				embed.WithDescription(translator.Get<GuildAuditNotificationTranslator>()
					.NotificationGuildAuditInternalCreate(guildEventTypeName, actor));
				break;
			case RestAction.Updated:
				embed.WithDescription(translator.Get<GuildAuditNotificationTranslator>()
					.NotificationGuildAuditInternalUpdate(guildEventTypeName, actor));
				break;
			case RestAction.Deleted:
				return embed.WithDescription(translator.Get<GuildAuditNotificationTranslator>()
					.NotificationGuildAuditInternalDelete(guildEventTypeName, actor));
			default:
				throw new ArgumentOutOfRangeException(nameof(action), action, null);
		}

		embed.AddField(translator.Get<BotTranslator>().Channel(), $"<#{config.ChannelId}>");

		if (config.PingRoles.Length > 0)
			embed.AddField(translator.Get<GuildAuditNotificationTranslator>().NotificationGuildAuditMentionRoles(),
				string.Join(" ", config.PingRoles.Select(x => $"<@&{x}>")));

		return embed;
	}
}