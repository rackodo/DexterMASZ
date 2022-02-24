using Bot.Abstractions;
using Bot.Data;
using Bot.Extensions;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Punishments.Extensions;
using Punishments.Translators;

namespace Punishments.Commands;

public class Report : Command<Report>
{
	public GuildConfigRepository GuildConfigRepository { get; set; }
	public DiscordSocketClient Client { get; set; }
	public IServiceProvider Services { get; set; }

	[MessageCommand("Report to moderators")]
	public async Task ReportCommand(IMessage msg)
	{
		GuildConfigRepository.AsUser(Identity);

		var guildConfig = await GuildConfigRepository.GetGuildConfig(Context.Guild.Id);

		try
		{
			var embed = await msg.CreateReportEmbed(Context.User, Services);

			await Client.SendEmbed(guildConfig.GuildId, guildConfig.StaffAnnouncements, embed);
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