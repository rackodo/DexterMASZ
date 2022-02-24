using Bot.Enums;
using Bot.Extensions;
using Bot.Services;
using Bot.Translators;
using Discord;
using Invites.Models;
using Invites.Translators;
using Microsoft.Extensions.DependencyInjection;

namespace Invites.Extensions;

public static class InviteEmbedCreator
{
	public static async Task<EmbedBuilder> CreateInviteEmbed(this UserInvite invite, IUser user, IServiceProvider provider)
	{
		var translation = provider.GetRequiredService<Translation>();

		await translation.SetLanguage(invite.GuildId);

		var embed = await EmbedCreator.CreateBasicEmbed(RestAction.Created, provider);

		embed.WithTitle(translation.Get<InviteTranslator>().UsedInvite())
		.WithAuthor(user)
		.WithDescription(user.Mention)
		.AddField(
			translation.Get<InviteNotificationTranslator>().Registered(),
			user.CreatedAt.DateTime,
			true
		).AddField(
			translation.Get<InviteNotificationTranslator>().Invite(),
			invite.UsedInvite,
			true
		)
		.WithFooter($"{translation.Get<BotTranslator>().GuildId()}: {invite.GuildId}");

		if (invite.InviteIssuerId != 0 && invite.InviteCreatedAt != null)
		{
			embed.AddField(translation.Get<InviteNotificationTranslator>().Created(), invite.InviteCreatedAt.Value)
				.AddField(translation.Get<InviteNotificationTranslator>().By(), $"<@{invite.InviteIssuerId}>");
		}

		return embed;
	}
}
