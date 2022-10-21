using Bot.Data;
using Bot.Exceptions;
using Bot.Models;
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
	public GuildConfigRepository GuildConfigRepository { get; set; }

	public GuildConfig guildConfig;

	public override async Task BeforeExecuteAsync(ICommandInfo command)
	{
		Logger.LogInformation(
			$"{Context.User.Id} used {command.Name} in {Context.Channel.Id} | {Context.Guild.Id} {Context.Guild.Name}");

		guildConfig = await GuildConfigRepository.GetGuildConfig(Context.Guild.Id);

		if (guildConfig != null)
			Translator.SetLanguage(guildConfig);
		else
			throw new UnregisteredGuildException(Context.Guild.Id);

		Identity = await IdentityManager.GetIdentity(Context.User);

		if (Identity == null)
			throw new InvalidIdentityException($"Failed to register command identity for '{Context.User.Id}'.");
	}
}