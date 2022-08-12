using Bot.Abstractions;
using Bot.Data;
using Bot.Enums;
using Bot.Exceptions;
using Bot.Extensions;
using Bot.Services;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using Humanizer;
using Punishments.Data;
using Punishments.Extensions;
using Punishments.Translators;
using System.Text;

namespace Punishments.Commands;

public class View : Command<View>
{
	public ModCaseRepository ModCaseRepository { get; set; }
	public SettingsRepository SettingsRepository { get; set; }
	public DiscordRest DiscordRest { get; set; }

	[SlashCommand("view", "View details of a mod case.")]
	public async Task ViewCommand([Summary("id", "the id of the case")] long caseId)
	{
		ModCaseRepository.AsUser(Identity);

		await Context.Interaction.RespondAsync("Getting mod cases...");

		try
		{
			var modCase = await ModCaseRepository.GetModCase(Context.Guild.Id, (int)caseId);

			if (!await Identity.HasPermission(ApiActionPermission.View, modCase))
			{
				await Context.Interaction.ModifyOriginalResponseAsync(message =>
					message.Content = Translator.Get<PunishmentTranslator>().NotAllowedToViewCase());
				return;
			}

			var config = await SettingsRepository.GetAppSettings();

			var embed = new EmbedBuilder()
				.WithUrl($"{config.GetServiceUrl()}/guilds/{modCase.GuildId}/cases/{modCase.CaseId}")
				.WithTimestamp(modCase.CreatedAt)
				.WithColor(Color.Blue)
				.WithTitle($"#{modCase.CaseId} {modCase.Title.Truncate(200)}")
				.WithDescription(modCase.Description.Truncate(2000))
				.AddField($"⚖️ - {Translator.Get<PunishmentTranslator>().Punishment()}",
					Translator.Get<PunishmentEnumTranslator>().Enum(modCase.PunishmentType), true);

			var suspect = await DiscordRest.FetchUserInfo(modCase.UserId, CacheBehavior.Default);

			if (suspect != null)
				embed.WithThumbnailUrl(suspect.GetAvatarOrDefaultUrl());

			if (modCase.PunishedUntil != null)
				embed.AddField($"⏰ - {Translator.Get<PunishmentTranslator>().PunishedUntil()}",
					modCase.PunishedUntil.Value.ToDiscordTs(), true);

			if (modCase.Labels.Length > 0)
			{
				StringBuilder labels = new();

				foreach (var label in modCase.Labels)
				{
					if (labels.ToString().Length + label.Length + 2 > 2000)
						break;

					labels.Append($"`{label}` ");
				}

				embed.AddField($"📜 - {Translator.Get<BotTranslator>().Labels()}", labels.ToString());
			}

			await Context.Interaction.ModifyOriginalResponseAsync(message =>
			{
				message.Content = "";
				message.Embed = embed.Build();
			});
		}
		catch (ResourceNotFoundException)
		{
			await Context.Interaction.ModifyOriginalResponseAsync(message =>
				message.Content = Translator.Get<BotTranslator>().NotFound());
		}
	}
}