using Discord;
using Humanizer;
using MASZ.Bot.Enums;
using MASZ.Bot.Extensions;
using MASZ.Bot.Services;
using MASZ.Bot.Translators;
using MASZ.UserNotes.Models;
using MASZ.UserNotes.Translators;
using Microsoft.Extensions.DependencyInjection;

namespace MASZ.UserNotes.Extensions;

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