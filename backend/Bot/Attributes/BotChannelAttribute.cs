using Bot.Data;
using Bot.Exceptions;
using Bot.Services;
using Discord.Interactions;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Bot.Translators;

namespace Bot.Attributes;

public class BotChannelAttribute : PreconditionAttribute
{
	public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context,
		ICommandInfo commandInfo, IServiceProvider services)
	{
		using var scope = services.CreateScope();
		var guildConfig = await scope.ServiceProvider
			.GetService<GuildConfigRepository>().GetGuildConfig(context.Guild.Id);
		var translator = scope.ServiceProvider.GetService<Translation>();
		translator.SetLanguage(guildConfig);

		if (!guildConfig.BotChannels.Contains(context.Channel.Id))
			return PreconditionResult.FromError(new UnauthorizedException(translator.Get<BotTranslator>().OnlyBotChannel()));

		return PreconditionResult.FromSuccess();
	}
}
