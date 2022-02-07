using Bot.Abstractions;
using Bot.Attributes;
using Bot.Data;
using Bot.Enums;
using Bot.Extensions;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Humanizer;
using Punishments.Data;
using Punishments.Enums;
using Punishments.Translators;
using System.Globalization;
using System.Text;

namespace Punishments.Commands;

public class Unmute : Command<Unmute>
{
	public ModCaseRepository ModCaseRepository { get; set; }
	public SettingsRepository SettingsRepository { get; set; }

	[Require(RequireCheck.GuildModerator, RequireCheck.GuildStrictModeMute)]
	[SlashCommand("unmute", "Un-mute a user by deactivating all his mod cases.")]
	public async Task UnmuteCommand([Summary("user", "User to un-mute")] IUser user)
	{
		ModCaseRepository.AsUser(Identity);

		var modCases = (await ModCaseRepository.GetCasesForGuildAndUser(Context.Guild.Id, user.Id))
			.Where(x => x.PunishmentActive && x.PunishmentType == PunishmentType.Mute).ToList();

		if (modCases.Count == 0)
		{
			await Context.Interaction.RespondAsync(Translator.Get<PunishmentTranslator>().NoActiveModCases());
			return;
		}

		StringBuilder interactionString = new();
		interactionString.AppendLine(Translator.Get<PunishmentTranslator>().FoundCasesForUnmute(modCases.Count));

		var config = await SettingsRepository.GetAppSettings();

		foreach (var modCase in modCases.Take(5))
		{
			var truncate = 50;

			if (modCase.PunishedUntil != null)
				truncate = 30;

			interactionString.Append($"- [#{modCase.CaseId} - {modCase.Title.Truncate(truncate)}]");
			interactionString.Append($"({config.ServiceBaseUrl}/guilds/{modCase.GuildId}/cases/{modCase.CaseId})");

			if (modCase.PunishedUntil != null)
				interactionString.Append(
					$" {Translator.Get<BotTranslator>().Until()} {modCase.PunishedUntil.Value.ToDiscordTs()}");

			interactionString.AppendLine();
		}

		if (modCases.Count > 5)
			interactionString.AppendLine(Translator.Get<BotTranslator>().AndXMore(modCases.Count - 5));

		var embed = new EmbedBuilder()
			.WithTitle(user.Username)
			.WithDescription(interactionString.ToString())
			.WithColor(Color.Orange);

		embed.AddField(Translator.Get<PunishmentTranslator>().Result(),
			Translator.Get<PunishmentTranslator>().WaitingForApproval());

		var button = new ComponentBuilder()
			.WithButton(Translator.Get<PunishmentTranslator>().DeleteMutes(), $"unmute-delete:{user.Id}")
			.WithButton(Translator.Get<PunishmentTranslator>().DeactivateMutes(), $"unmute-deactivate:{user.Id}",
				ButtonStyle.Secondary)
			.WithButton(Translator.Get<PunishmentTranslator>().Cancel(), "unmute-cancel", ButtonStyle.Danger);

		await Context.Interaction.RespondAsync(embed: embed.Build(), components: button.Build());
	}

	[ComponentInteraction("unmute-delete:*")]
	public async Task DeleteMute(string userId)
	{
		var button = new ComponentBuilder()
			.WithButton(Translator.Get<PunishmentTranslator>().PublicNotification(), $"unmute-conf-delete:1,{userId}")
			.WithButton(Translator.Get<PunishmentTranslator>().NoPublicNotification(), $"unmute-conf-delete:0,{userId}",
				ButtonStyle.Secondary)
			.WithButton(Translator.Get<PunishmentTranslator>().Cancel(), "unmute-cancel", ButtonStyle.Danger);

		if (Context.Interaction is SocketMessageComponent castInteraction)
		{
			var embed = castInteraction.Message.Embeds.FirstOrDefault().ToEmbedBuilder().WithColor(Color.Red);

			embed.Fields = new List<EmbedFieldBuilder>
			{
				new EmbedFieldBuilder().WithName(Translator.Get<PunishmentTranslator>().Result())
					.WithValue(Translator.Get<PunishmentTranslator>().WaitingForApproval()),

				new EmbedFieldBuilder().WithName(Translator.Get<PunishmentTranslator>().PublicNotification())
					.WithValue(Translator.Get<PunishmentTranslator>().ShouldSendPublicNotification())
			};

			await castInteraction.UpdateAsync(message =>
			{
				message.Embed = embed.Build();
				message.Components = button.Build();
			});
		}
	}

	[ComponentInteraction("unmute-conf-delete:*,*")]
	public async Task DeleteMuteConfirmation(string isPublic, string userId)
	{
		ModCaseRepository.AsUser(Identity);

		var modCases = (await ModCaseRepository.GetCasesForGuildAndUser(Context.Guild.Id, Convert.ToUInt64(userId)))
			.Where(x => x.PunishmentActive && x.PunishmentType == PunishmentType.Mute).ToList();

		foreach (var modCase in modCases)
			await ModCaseRepository.DeleteModCase(modCase.GuildId, modCase.CaseId, false, true, isPublic == "1");

		if (Context.Interaction is SocketMessageComponent castInteraction)
		{
			var embed = castInteraction.Message.Embeds.FirstOrDefault().ToEmbedBuilder()
				.WithColor(new Color(Convert.ToUInt32(int.Parse("7289da", NumberStyles.HexNumber))));

			embed.Fields = new List<EmbedFieldBuilder>
			{
				new EmbedFieldBuilder().WithName(Translator.Get<PunishmentTranslator>().Result())
					.WithValue(Translator.Get<PunishmentTranslator>().MutesDeleted())
			};

			await castInteraction.UpdateAsync(message =>
			{
				message.Embed = embed.Build();
				message.Components = new ComponentBuilder().Build();
			});
		}
	}

	[ComponentInteraction("unmute-deactivate:*")]
	public async Task Deactivatemute(string userId)
	{
		ModCaseRepository.AsUser(Identity);

		var modCases = (await ModCaseRepository.GetCasesForGuildAndUser(Context.Guild.Id, Convert.ToUInt64(userId)))
			.Where(x => x.PunishmentActive && x.PunishmentType == PunishmentType.Mute).ToList();

		await ModCaseRepository.DeactivateModCase(modCases.ToArray());

		if (Context.Interaction is SocketMessageComponent castInteraction)
		{
			var embed = castInteraction.Message.Embeds.FirstOrDefault().ToEmbedBuilder().WithColor(Color.Green);

			embed.Fields = new List<EmbedFieldBuilder>
			{
				new EmbedFieldBuilder().WithName(Translator.Get<PunishmentTranslator>().Result())
					.WithValue(Translator.Get<PunishmentTranslator>().MutesDeactivated())
			};

			await castInteraction.UpdateAsync(message =>
			{
				message.Embed = embed.Build();
				message.Components = new ComponentBuilder().Build();
			});
		}
	}

	[ComponentInteraction("unmute-cancel")]
	public async Task UnmuteCancel()
	{
		if (Context.Interaction is SocketMessageComponent castInteraction)
		{
			var embed = castInteraction.Message.Embeds.FirstOrDefault().ToEmbedBuilder().WithColor(Color.Red);

			embed.Fields = new List<EmbedFieldBuilder>
			{
				new EmbedFieldBuilder().WithName(Translator.Get<PunishmentTranslator>().Result())
					.WithValue(Translator.Get<PunishmentTranslator>().Canceled())
			};

			await castInteraction.UpdateAsync(message =>
			{
				message.Embed = embed.Build();
				message.Components = new ComponentBuilder().Build();
			});
		}
	}
}