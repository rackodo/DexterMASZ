using Discord;
using Humanizer;
using Bot.Enums;
using Bot.Extensions;
using Bot.Services;
using Bot.Translators;
using UserNotes.Models;
using UserNotes.Translators;
using Microsoft.Extensions.DependencyInjection;

namespace UserNotes.Extensions;

public static class UserNoteEmbedCreator
{
	public static async Task<EmbedBuilder> CreateUserNoteEmbed(this UserNote userNote, RestAction action, IUser actor,
		IUser target, IServiceProvider provider)
	{
		var translator = provider.GetRequiredService<Translation>();

		await translator.SetLanguage(userNote.GuildId);

		var embed = await EmbedCreator.CreateBasicEmbed(action, provider, actor);

		if (target != null)
			embed.WithThumbnailUrl(target.GetAvatarOrDefaultUrl());

		embed.AddField($"**{translator.Get<BotTranslator>().Description()}**", userNote.Description.Truncate(1000))
			.WithTitle($"{translator.Get<UserNoteTranslator>().UserNote()} #{userNote.Id}")
			.WithFooter(
				$"{translator.Get<BotTranslator>().UserId()}: {userNote.UserId} | {translator.Get<UserNoteTranslator>().UserNoteId()}: {userNote.Id}");

		return embed;
	}
}