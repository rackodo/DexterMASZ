using Bot.Abstractions;
using Bot.Data;
using Bot.Services;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using Humanizer;
using Microsoft.Extensions.Logging;
using Punishments.Translators;
using System.Text;

namespace Punishments.Commands;

public class Report : Command<Report>
{
	public GuildConfigRepository GuildConfigRepository { get; set; }
	public DiscordRest DiscordRest { get; set; }

	[MessageCommand("Report to moderators")]
	public async Task ReportCommand(IMessage msg)
	{
		GuildConfigRepository.AsUser(Identity);

		var guildConfig = await GuildConfigRepository.GetGuildConfig(Context.Guild.Id);

		if (string.IsNullOrEmpty(guildConfig.AdminWebhook))
		{
			await Context.Interaction.RespondAsync(Translator.Get<BotTranslator>().NoWebhookConfigured(),
				ephemeral: true);
			return;
		}

		StringBuilder sb = new();
		sb.AppendLine(Translator.Get<PunishmentTranslator>()
			.ReportContent(Context.User, msg, msg.Channel as ITextChannel));

		if (!string.IsNullOrEmpty(msg.Content))
		{
			sb.Append("```\n");
			sb.Append(msg.Content.Truncate(1024));
			sb.Append("\n``` ");
		}

		if (msg.Attachments.Count > 0)
		{
			sb.AppendLine(Translator.Get<BotTranslator>().Attachments());

			foreach (var attachment in msg.Attachments.Take(5))
				sb.Append($"- <{attachment.Url}>\n");

			if (msg.Attachments.Count > 5)
				sb.AppendLine(Translator.Get<BotTranslator>().AndXMore(msg.Attachments.Count - 5));
		}

		try
		{
			await DiscordRest.ExecuteWebhook(guildConfig.AdminWebhook, null, sb.ToString(), AllowedMentions.None);
		}
		catch (Exception e)
		{
			Logger.LogError(e, "Failed to send internal notification to moderators for report command.");
			await Context.Interaction.RespondAsync(Translator.Get<PunishmentTranslator>().ReportFailed(),
				ephemeral: true);
			return;
		}

		await Context.Interaction.RespondAsync(Translator.Get<PunishmentTranslator>().ReportSent(), ephemeral: true);
	}
}