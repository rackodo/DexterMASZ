using Bot.Exceptions;
using Bot.Services;
using Discord.Interactions;
using Microsoft.Extensions.Logging;

namespace Bot.Abstractions;

public abstract class Command<T> : InteractionModuleBase<SocketInteractionContext>
{
	public Identity Identity;
	public ILogger<T> Logger { get; set; }
	public Translation Translator { get; set; }
	public IdentityManager IdentityManager { get; set; }

	public override async Task BeforeExecuteAsync(ICommandInfo command)
	{
		Logger.LogInformation(
			$"{Context.User.Id} used {command.Name} in {Context.Channel.Id} | {Context.Guild.Id} {Context.Guild.Name}");

		if (Context.Guild != null)
			await Translator.SetLanguage(Context.Guild.Id);

		Identity = await IdentityManager.GetIdentity(Context.User);

		if (Identity == null)
			throw new InvalidIdentityException($"Failed to register command identity for '{Context.User.Id}'.");
	}

	public async Task<bool> EnsureBotChannel(Models.GuildConfig guildConfig)
	{
		if (!guildConfig.BotChannels.Contains(Context.Channel.Id))
		{
			await DeclineCommand(Translator.Get<Translators.BotTranslator>().OnlyBotChannel(), true);
			return false;
		}
		return true;
	}

	public async Task DeclineCommand(string reply, bool ephemeral = false)
	{
		await RespondAsync(reply, ephemeral: ephemeral);
	}
}