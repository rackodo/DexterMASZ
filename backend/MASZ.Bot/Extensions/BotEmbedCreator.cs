using Discord;
using MASZ.Bot.Enums;
using MASZ.Bot.Models;
using MASZ.Bot.Services;
using MASZ.Bot.Translators;
using Microsoft.Extensions.DependencyInjection;

namespace MASZ.Bot.Extensions;

public static class BotEmbedCreator
{
	public static async Task<EmbedBuilder> CreateTipsEmbedForNewGuilds(this GuildConfig guildConfig,
		IServiceProvider provider)
	{
		var translator = provider.GetRequiredService<Translation>();

		await translator.SetLanguage(guildConfig.GuildId);

		var embed = (await EmbedCreator.CreateBasicEmbed(RestAction.Created, provider))
			.WithTitle(translator.Get<BotNotificationTranslator>().NotificationRegisterWelcomeToMasz())
			.WithDescription(translator.Get<BotNotificationTranslator>().NotificationRegisterDescriptionThanks())
			.AddField(
				$"⭐ {translator.Get<BotTranslator>().Features()}",
				translator.Get<BotNotificationTranslator>().NotificationRegisterUseFeaturesCommand()
			).AddField(
				$"🌎 {translator.Get<BotTranslator>().Language()}",
				translator.Get<BotNotificationTranslator>()
					.NotificationRegisterDefaultLanguageUsed(guildConfig.PreferredLanguage.ToString())
			).AddField(
				$"🕐 {translator.Get<BotTranslator>().Timestamps()}",
				translator.Get<BotNotificationTranslator>().NotificationRegisterConfusingTimestamps()
			).AddField(
				$"🤝 {translator.Get<BotTranslator>().Support()}",
				translator.Get<BotNotificationTranslator>().NotificationRegisterSupport()
			).WithFooter($"{translator.Get<BotTranslator>().GuildId()}: {guildConfig.GuildId}");

		return embed;
	}
}