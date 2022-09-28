using Bot.Abstractions;
using Bot.Data;
using Bot.Enums;
using Bot.Services;
using Discord;
using Punishments.Data;
using Punishments.Extensions;
using Punishments.Models;

namespace Punishments.Abstractions;

public class PunishmentCommand<T> : Command<T>
{

	public ModCaseRepository ModCaseRepository { get; set; }
	public SettingsRepository SettingsRepository { get; set; }
	public GuildConfigRepository GuildConfigRepository { get; set; }
	public IServiceProvider ServiceProvider { get; set; }
	public DiscordRest DiscordRest { get; set; }

	public async Task RunModcase(ModCase modCase)
	{
		ModCaseRepository.AsUser(Identity);
		GuildConfigRepository.AsUser(Identity);

		var guildConfig = await GuildConfigRepository.GetGuildConfig(Context.Guild.Id);

		await Context.Interaction.DeferAsync(ephemeral: !guildConfig.StaffChannels.Contains(Context.Channel.Id));

		var (created, result) =
			await ModCaseRepository.CreateModCase(modCase);

		var caseCount =
			(await ModCaseRepository.GetCasesForGuildAndUser(Context.Guild.Id, modCase.UserId)).Count;

		var caseUser = await DiscordRest.FetchUserInfo(modCase.UserId, CacheBehavior.Default);

		var modUser = await DiscordRest.FetchUserInfo(modCase.ModId, CacheBehavior.Default);

		var settings = await SettingsRepository.GetAppSettings();

		var embed = await modCase.CreateNewModCaseEmbed(modUser, guildConfig, result, ServiceProvider, caseUser);

		var url = $"{settings.GetServiceUrl()}/guilds/{created.GuildId}/cases/{created.CaseId}";

		var buttons = new ComponentBuilder().WithButton(label: "View Case", style: ButtonStyle.Link, url: url);

		await Context.Interaction.ModifyOriginalResponseAsync((MessageProperties msg) =>
		{
			msg.Embed = embed.Build();
			msg.Components = buttons.Build();
		});
	}

}
