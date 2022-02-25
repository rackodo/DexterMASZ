using Bot.Abstractions;
using Bot.Enums;
using Bot.Extensions;
using Discord;
using Discord.Interactions;

namespace DexterSlash.Commands.ModeratorCommands;

public class UserDM : Command<UserDM>
{
	public IServiceProvider ServiceProvider { get; set; }

	[SlashCommand("dm", "Sends a direct message to a user specified.")]
	public async Task UserDMCommand(
		[Summary("user", "The user you wish to be direct messaged.")]
		IUser user,
		[Summary("message", "The message you wish to be sent to the user.")]
		string message)
	{
		if (user is null)
		{
			var embed = (await EmbedCreator.CreateActionEmbed(RestAction.Deleted, ServiceProvider))
				.WithTitle("Unable to find given user!")
				.WithDescription("This may be due to caching! Try using their ID if you haven't.");

			await Context.Interaction.RespondAsync(embed: embed.Build(), ephemeral: true);

			return;
		}

		if (string.IsNullOrEmpty(message))
		{
			var embed = (await EmbedCreator.CreateActionEmbed(RestAction.Deleted, ServiceProvider))
				.WithTitle("Empty message!")
				.WithDescription("I received an empty message. It would be rude for me to send that; I believe.");

			await Context.Interaction.RespondAsync(embed: embed.Build(), ephemeral: true);

			return;
		}

		await CreateEmbed(EmojiEnum.Love)
			.WithTitle("User DM")
			.WithDescription(message)
			.AddField("Recipient", user.GetUserInformation())
			.AddField("Sent By", Context.User.GetUserInformation())
			.SendDMAttachedEmbed(
				Context.Interaction,
				user,
				CreateEmbed(EmojiEnum.Unknown)
					.WithTitle($"Message From {Context.Guild.Name}")
					.WithDescription(message)
			);
	}
}
