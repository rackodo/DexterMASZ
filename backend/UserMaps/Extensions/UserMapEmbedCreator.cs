using Bot.Enums;
using Bot.Extensions;
using Bot.Services;
using Bot.Translators;
using Discord;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using UserMaps.Models;
using UserMaps.Translators;

namespace UserMaps.Extensions;

public static class UserMapEmbedCreator
{
	public static async Task<EmbedBuilder> CreateUserMapEmbed(this UserMap userMaps, RestAction action, IUser actor,
		IServiceProvider provider)
	{
		var translator = provider.GetRequiredService<Translation>();

		await translator.SetLanguage(userMaps.GuildId);

		var embed = await EmbedCreator.CreateActionEmbed(action, provider, actor);

		embed.AddField($"**{translator.Get<BotTranslator>().Description()}**", userMaps.Reason.Truncate(1000))
			.WithTitle($"{translator.Get<UserMapTranslator>().UserMap()} #{userMaps.Id}")
			.WithDescription(translator.Get<UserMapTranslator>().UserMapBetween(userMaps));

		embed.WithFooter(
			$"{translator.Get<BotTranslator>().User()} A: {userMaps.UserA} | {translator.Get<BotTranslator>().User()} B: {userMaps.UserB} | {translator.Get<UserMapTranslator>().UserMapId()}: {userMaps.Id}");

		return embed;
	}
}