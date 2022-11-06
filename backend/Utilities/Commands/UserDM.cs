using Bot.Abstractions;
using Bot.Attributes;
using Bot.Enums;
using Bot.Extensions;
using Bot.Services;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using System.Text;

namespace DexterSlash.Commands.ModeratorCommands;

public class UserDM : Command<UserDM>
{
	public IServiceProvider ServiceProvider { get; set; }
	public DiscordRest DiscordRest { get; set; }

	[Require(RequireCheck.GuildModerator)]
	[SlashCommand("dm", "Sends a direct message to a user specified.")]
	public async Task UserDMCommand(
		[Summary("user", "The user you wish to be direct messaged.")]
		IUser user,
		[Summary("message", "The message you wish to be sent to the user.")]
		string message)
	{
		await Context.Interaction.DeferAsync(ephemeral: !GuildConfig.StaffChannels.Contains(Context.Channel.Id));

		if (user is null)
		{
			var embed = new EmbedBuilder()
				.WithCurrentTimestamp()
				.WithColor(Color.Red)
				.WithTitle("Unable to find given user!")
				.WithDescription("This may be due to caching! Try using their ID if you haven't.");

			await Context.Interaction.RespondAsync(embed: embed.Build(), ephemeral: true);

			return;
		}

		if (string.IsNullOrEmpty(message))
		{
			var embed = new EmbedBuilder()
				.WithCurrentTimestamp()
				.WithColor(Color.Red)
				.WithTitle("Empty message!")
				.WithDescription("I received an empty message. It would be rude for me to send that; I believe.");

			await Context.Interaction.RespondAsync(embed: embed.Build(), ephemeral: true);

			return;
		}

		StringBuilder recipient = new();
		recipient.AppendLine(
			$"> **{Translator.Get<BotTranslator>().User()}:** {user.Username}#{user.Discriminator} - {user.Mention}");
		recipient.AppendLine($"> **{Translator.Get<BotTranslator>().Id()}:** `{user.Id}`");

		StringBuilder sender = new();

		sender.AppendLine(
			$"> **{Translator.Get<BotTranslator>().User()}:** {Context.User.Username}#{Context.User.Discriminator} - {Context.User.Mention}");
		sender.AppendLine($"> **{Translator.Get<BotTranslator>().Id()}:** `{Context.User.Id}`");

		var sendEmbed = new EmbedBuilder()
			.WithCurrentTimestamp()
			.WithColor(Color.Green)
			.WithTitle("User DM")
			.WithDescription(message)
			.AddField("Recipient", recipient.ToString())
			.AddField("Sent By", sender.ToString());

		try
		{
			var channel = await DiscordRest.CreateDmChannel(user.Id);

			await channel.SendMessageAsync(embed: EmbedCreator.CreateColoredEmbed(Color.Green, typeof(UserDM))
					.WithTitle($"Message From {Context.Guild.Name}")
					.WithDescription(message).Build());
		}
		catch (Exception)
		{
			sendEmbed.AddField("Failed", "They might have DMs disabled or me blocked!")
				.WithColor(Color.Red);
		}

		await Context.Interaction.ModifyOriginalResponseAsync(message =>
		{
			message.Embed = sendEmbed.Build();
		});
	}
}
