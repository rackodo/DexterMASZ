using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Humanizer;
using Bot.Abstractions;
using Bot.Attributes;
using Bot.Data;
using Bot.Enums;
using Bot.Extensions;
using Bot.Translators;
using Punishments.Data;
using Punishments.Enums;
using Punishments.Translators;
using System.Globalization;
using System.Text;

namespace Punishments.Commands;

public class Unban : Command<Unban>
{
	public ModCaseRepository ModCaseRepository { get; set; }
	public SettingsRepository SettingsRepository { get; set; }

	[Require(RequireCheck.GuildModerator, RequireCheck.GuildStrictModeBan)]
	[SlashCommand("unban", "Un-ban a user by deactivating all his mod cases.")]
	public async Task UnbanCommand([Summary("user", "User to un-ban")] IUser user)
	{
		ModCaseRepository.AsUser(Identity);

		var modCases = (await ModCaseRepository.GetCasesForGuildAndUser(Context.Guild.Id, user.Id))
			.Where(x => x.PunishmentActive && x.PunishmentType == PunishmentType.Ban).ToList();

		if (modCases.Count == 0)
		{
			await Context.Interaction.RespondAsync(Translator.Get<PunishmentTranslator>().NoActiveModCases());
			return;
		}

		StringBuilder interactionString = new();
		interactionString.AppendLine(Translator.Get<PunishmentTranslator>().FoundCasesForUnban(modCases.Count));

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
			.WithButton(Translator.Get<PunishmentTranslator>().DeleteBans(), $"unban-delete:{user.Id}")
			.WithButton(Translator.Get<PunishmentTranslator>().DeactivateBans(), $"unban-deactivate:{user.Id}",
				ButtonStyle.Secondary)
			.WithButton(Translator.Get<PunishmentTranslator>().Cancel(), "unban-cancel", ButtonStyle.Danger);

		await Context.Interaction.RespondAsync(embed: embed.Build(), components: button.Build());
	}

	[ComponentInteraction("unban-delete:*")]
	public async Task DeleteBan(string userId)
	{
		var button = new ComponentBuilder()
			.WithButton(Translator.Get<PunishmentTranslator>().PublicNotification(), $"unban-conf-delete:1,{userId}")
			.WithButton(Translator.Get<PunishmentTranslator>().NoPublicNotification(), $"unban-conf-delete:0,{userId}",
				ButtonStyle.Secondary)
			.WithButton(Translator.Get<PunishmentTranslator>().Cancel(), "unban-cancel", ButtonStyle.Danger);

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

	[ComponentInteraction("unban-conf-delete:*,*")]
	public async Task DeleteBanConfirmation(string isPublic, string userId)
	{
		ModCaseRepository.AsUser(Identity);

		var modCases = (await ModCaseRepository.GetCasesForGuildAndUser(Context.Guild.Id, Convert.ToUInt64(userId)))
			.Where(x => x.PunishmentActive && x.PunishmentType == PunishmentType.Ban).ToList();

		foreach (var modCase in modCases)
			await ModCaseRepository.DeleteModCase(modCase.GuildId, modCase.CaseId, false, true, isPublic == "1");

		if (Context.Interaction is SocketMessageComponent castInteraction)
		{
			var embed = castInteraction.Message.Embeds.FirstOrDefault().ToEmbedBuilder()
				.WithColor(new Color(Convert.ToUInt32(int.Parse("7289da", NumberStyles.HexNumber))));

			embed.Fields = new List<EmbedFieldBuilder>
			{
				new EmbedFieldBuilder().WithName(Translator.Get<PunishmentTranslator>().Result())
					.WithValue(Translator.Get<PunishmentTranslator>().BansDeleted())
			};

			await castInteraction.UpdateAsync(message =>
			{
				message.Embed = embed.Build();
				message.Components = new ComponentBuilder().Build();
			});
		}
	}

	[ComponentInteraction("unban-deactivate:*")]
	public async Task DeactivateBan(string userId)
	{
		ModCaseRepository.AsUser(Identity);

		var modCases = (await ModCaseRepository.GetCasesForGuildAndUser(Context.Guild.Id, Convert.ToUInt64(userId)))
			.Where(x => x.PunishmentActive && x.PunishmentType == PunishmentType.Ban).ToList();

		await ModCaseRepository.DeactivateModCase(modCases.ToArray());

		if (Context.Interaction is SocketMessageComponent castInteraction)
		{
			var embed = castInteraction.Message.Embeds.FirstOrDefault().ToEmbedBuilder().WithColor(Color.Green);

			embed.Fields = new List<EmbedFieldBuilder>
			{
				new EmbedFieldBuilder().WithName(Translator.Get<PunishmentTranslator>().Result())
					.WithValue(Translator.Get<PunishmentTranslator>().BansDeactivated())
			};

			await castInteraction.UpdateAsync(message =>
			{
				message.Embed = embed.Build();
				message.Components = new ComponentBuilder().Build();
			});
		}
	}

	[ComponentInteraction("unban-cancel")]
	public async Task UnbanCancel()
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