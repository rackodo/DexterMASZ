﻿using Bot.Abstractions;
using Bot.Data;
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
	public IServiceProvider ServiceProvider { get; set; }
	public DiscordRest DiscordRest { get; set; }

	public async Task RunModcase(ModCase modCase)
	{
		ModCaseRepository.AsUser(Identity);
		GuildConfigRepository.AsUser(Identity);

		await Context.Interaction.DeferAsync(ephemeral: !GuildConfig.StaffChannels.Contains(Context.Channel.Id));

		var (created, result, finalWarned) =
			await ModCaseRepository.CreateModCase(modCase);

		if (finalWarned)
		{
			var textChannel = Context.Guild.GetTextChannel(GuildConfig.StaffAnnouncements);

			await Context.Interaction.ModifyOriginalResponseAsync(msg =>
				msg.Content = $"Final warning hit! Please check {textChannel.Mention} for more information."
			);
		}
		else
		{
			var (embed, buttons) = await modCase.CreateNewModCaseEmbed(GuildConfig, await SettingsRepository.GetAppSettings(), result, DiscordRest, ServiceProvider);

			await Context.Interaction.ModifyOriginalResponseAsync((MessageProperties msg) =>
			{
				msg.Embed = embed.Build();
				msg.Components = buttons.Build();
			});
		}
	}

}
