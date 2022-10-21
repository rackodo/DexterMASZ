using Bot.Abstractions;
using Bot.Data;
using Bot.Services;
using Discord;
using Punishments.Data;
using Punishments.Enums;
using Punishments.Extensions;
using Punishments.Models;

namespace Punishments.Abstractions;

public class PunishmentCommand<T> : Command<T>
{

	public ModCaseRepository ModCaseRepository { get; set; }
	public SettingsRepository SettingsRepository { get; set; }
	public IServiceProvider ServiceProvider { get; set; }
	public DiscordRest DiscordRest { get; set; }

	public async Task RunModcase(ModCase modCase)
	{
		ModCaseRepository.AsUser(Identity);
		GuildConfigRepository.AsUser(Identity);

		await Context.Interaction.DeferAsync(ephemeral: !guildConfig.StaffChannels.Contains(Context.Channel.Id));

		var (created, result) =
			await ModCaseRepository.CreateModCase(modCase);

		var cases = await ModCaseRepository.GetCasesForGuildAndUser(Context.Guild.Id, modCase.UserId);

		var caseUser = await DiscordRest.FetchUserInfo(modCase.UserId, false);

		var modUser = await DiscordRest.FetchUserInfo(modCase.ModId, false);

		var settings = await SettingsRepository.GetAppSettings();

		var embed = (await modCase.CreateNewModCaseEmbed(modUser, guildConfig, result, ServiceProvider, caseUser));

		var url = $"{settings.GetServiceUrl()}/guilds/{created.GuildId}/cases/{created.CaseId}";

		var buttons = new ComponentBuilder().WithButton(label: "View Case", style: ButtonStyle.Link, url: url).Build();

		if (cases.Where(c => c.Valid.GetValueOrDefault() && c.PunishmentType == PunishmentType.FinalWarn).Any())
		{
			var textChannel = Context.Guild.GetTextChannel(guildConfig.StaffAnnouncements);

			embed.WithTitle($"ON FINAL WARN: {embed.Title}");

			await textChannel.SendMessageAsync(text: "⚠️ FINAL WARNING TRIGGERED ⚠️", embed: embed.Build(), components: buttons);

			await Context.Interaction.ModifyOriginalResponseAsync(msg =>
				msg.Content = $"Final warning hit! Please check {textChannel.Mention} for more information."
			);
		}
		else
			await Context.Interaction.ModifyOriginalResponseAsync((MessageProperties msg) =>
			{
				msg.Embed = embed.Build();
				msg.Components = buttons;
			});
	}

}
