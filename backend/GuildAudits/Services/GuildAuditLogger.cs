using Bot.Abstractions;
using Bot.Data;
using Bot.Exceptions;
using Bot.Extensions;
using Bot.Services;
using Bot.Translators;
using Discord;
using Discord.WebSocket;
using GuildAudits.Data;
using GuildAudits.Enums;
using GuildAudits.Translators;
using Humanizer;
using Invites.Events;
using Invites.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace GuildAudits.Services;

public class GuildAuditer : Event
{
	private readonly DiscordSocketClient _client;
	private readonly InviteEventHandler _eventHandler;
	private readonly IServiceProvider _serviceProvider;

	public GuildAuditer(DiscordSocketClient client, IServiceProvider serviceProvider,
		InviteEventHandler eventHandler)
	{
		_client = client;
		_serviceProvider = serviceProvider;
		_eventHandler = eventHandler;
	}

	public void RegisterEvents()
	{
		_client.UserBanned += HandleBanAdded;
		_client.UserUnbanned += HandleBanRemoved;
		_client.InviteCreated += HandleInviteCreated;
		_client.UserJoined += HandleUserJoined;
		_client.UserLeft += HandleUserRemoved;
		_client.MessageReceived += HandleMessageSent;
		_client.MessageDeleted += HandleMessageDeleted;
		_client.MessageUpdated += HandleMessageUpdated;
		_client.ThreadCreated += HandleThreadCreated;
		_client.UserUpdated += HandleUsernameUpdated;
		_client.GuildMemberUpdated += HandleGuildUserUpdated;

		_eventHandler.OnInviteDeleted += HandleInviteDeleted;
	}

	public async Task SendEmbed(EmbedBuilder embed, ulong guildId, GuildAuditEvent eventType)
	{
		using var scope = _serviceProvider.CreateScope();

		var guildConfigRepository = scope.ServiceProvider.GetRequiredService<GuildConfigRepository>();

		embed.WithColor(eventType switch
		{
			GuildAuditEvent.MessageSent => Color.Green,
			GuildAuditEvent.MessageUpdated => Color.Orange,
			GuildAuditEvent.MessageDeleted => Color.Red,
			GuildAuditEvent.UsernameUpdated => Color.Orange,
			GuildAuditEvent.AvatarUpdated => Color.Orange,
			GuildAuditEvent.NicknameUpdated => Color.Orange,
			GuildAuditEvent.UserRolesUpdated => Color.Orange,
			GuildAuditEvent.UserJoined => Color.Green,
			GuildAuditEvent.UserRemoved => Color.Red,
			GuildAuditEvent.BanAdded => Color.Red,
			GuildAuditEvent.BanRemoved => Color.Green,
			GuildAuditEvent.InviteCreated => Color.Green,
			GuildAuditEvent.InviteDeleted => Color.Red,
			GuildAuditEvent.ThreadCreated => Color.Green,
			_ => throw new NotImplementedException()
		})
			.WithCurrentTimestamp();

		try
		{
			var guildConfig = await guildConfigRepository.GetGuildConfig(guildId);

			if (guildConfig == null)
				return;

			var auditLogRepository = scope.ServiceProvider.GetRequiredService<GuildAuditConfigRepository>();

			var auditLogConfig = await auditLogRepository.GetConfigsByGuildAndType(guildId, eventType);

			if (auditLogConfig == null)
				return;

			if (embed.Footer == null)
				embed.WithFooter(auditLogConfig.GuildAuditEvent.Humanize());
			else
				embed.WithFooter(embed.Footer.Text + $" | {auditLogConfig.GuildAuditEvent.Humanize()}");

			StringBuilder rolePings = new();

			foreach (var role in auditLogConfig.PingRoles)
				rolePings.Append($"<@&{role}> ");

			if (await _client.GetChannelAsync(auditLogConfig.ChannelId) is ITextChannel channel)
			{
				await channel.SendMessageAsync(rolePings.ToString(), embed: embed.Build());
			}
		}
		catch (ResourceNotFoundException)
		{
		}
	}

	public async Task HandleGuildUserUpdated(Cacheable<SocketGuildUser, ulong> oldU, SocketGuildUser newU)
	{
		var oldUser = await oldU.GetOrDownloadAsync();

		if (oldUser.Nickname != newU.Nickname)
			await HandleNicknameUpdated(oldUser, newU, newU.Guild.Id);

		if (oldUser.AvatarId != newU.AvatarId)
			await HandleAvatarUpdated(oldUser, newU, newU.Guild.Id);

		if (!Equals(oldUser.Roles, newU.Roles))
			await HandleUserRolesUpdated(newU, oldUser.Roles, newU.Roles, newU.Guild.Id);
	}

	public async Task HandleAvatarUpdated(IGuildUser oldU, IGuildUser newU, ulong guildId)
	{
		using var scope = _serviceProvider.CreateScope();

		var translator = scope.ServiceProvider.GetRequiredService<Translation>();
		await translator.SetLanguage(guildId);

		StringBuilder description = new();
		description.AppendLine(
			$"> **{translator.Get<BotTranslator>().User()}:** {newU.Username}#{newU.Discriminator} - {newU.Mention}");
		description.AppendLine($"> **{translator.Get<BotTranslator>().Id()}:** `{newU.Id}`");

		var embed = new EmbedBuilder()
			.WithTitle(translator.Get<GuildAuditTranslator>().AvatarUpdated())
			.WithDescription(description.ToString())
			.WithImageUrl(newU.GetAvatarOrDefaultUrl())
			.AddField(
				translator.Get<GuildAuditTranslator>().Old(),
				oldU.GetAvatarOrDefaultUrl(),
				true
			).AddField(
				translator.Get<GuildAuditTranslator>().New(),
				newU.GetAvatarOrDefaultUrl(),
				true
			)
			.WithAuthor(newU);

		await SendEmbed(embed, guildId, GuildAuditEvent.AvatarUpdated);
	}

	public async Task HandleNicknameUpdated(IGuildUser oldU, IGuildUser newU, ulong guildId)
	{
		using var scope = _serviceProvider.CreateScope();

		var translator = scope.ServiceProvider.GetRequiredService<Translation>();
		await translator.SetLanguage(guildId);

		StringBuilder description = new();
		description.AppendLine(
			$"> **{translator.Get<BotTranslator>().User()}:** {newU.Username}#{newU.Discriminator} - {newU.Mention}");
		description.AppendLine($"> **{translator.Get<BotTranslator>().Id()}:** `{newU.Id}`");

		var embed = new EmbedBuilder()
			.WithTitle(translator.Get<GuildAuditTranslator>().NicknameUpdated())
			.WithDescription(description.ToString())
			.AddField(
				translator.Get<GuildAuditTranslator>().Old(),
				string.IsNullOrEmpty(oldU.Nickname)
					? $"`{translator.Get<GuildAuditTranslator>().Empty()}`"
					: oldU.Nickname,
				true
			)
			.AddField(
				translator.Get<GuildAuditTranslator>().New(),
				string.IsNullOrEmpty(newU.Nickname)
					? $"`{translator.Get<GuildAuditTranslator>().Empty()}`"
					: newU.Nickname,
				true
			)
			.WithAuthor(newU);

		await SendEmbed(embed, guildId, GuildAuditEvent.NicknameUpdated);
	}

	public async Task HandleUserRolesUpdated(IGuildUser user, IReadOnlyCollection<SocketRole> roleOld,
		IReadOnlyCollection<SocketRole> roleNew, ulong guildId)
	{
		using var scope = _serviceProvider.CreateScope();

		var translator = scope.ServiceProvider.GetRequiredService<Translation>();
		await translator.SetLanguage(guildId);

		StringBuilder description = new();
		description.AppendLine(
			$"> **{translator.Get<BotTranslator>().User()}:** {user.Username}#{user.Discriminator} - {user.Mention}");
		description.AppendLine($"> **{translator.Get<BotTranslator>().Id()}:** `{user.Id}`");

		var embed = new EmbedBuilder()
			.WithTitle(translator.Get<GuildAuditTranslator>().RolesUpdated())
			.WithDescription(description.ToString())
			.WithAuthor(user);

		var addedRoles = roleNew.Except(roleOld).ToList();
		var removedRoles = roleOld.Except(roleNew).ToList();

		if (removedRoles.Count > 0)
			embed.AddField(
				translator.Get<GuildAuditTranslator>().Removed(),
				string.Join(" ", removedRoles.Select(x => x.Mention)),
				true
			);

		if (addedRoles.Count > 0)
			embed.AddField(
				translator.Get<GuildAuditTranslator>().Added(),
				string.Join(" ", addedRoles.Select(x => x.Mention)),
				true
			);

		if (addedRoles.Count + removedRoles.Count > 0)
			await SendEmbed(embed, guildId, GuildAuditEvent.UserRolesUpdated);
	}

	public async Task HandleUsernameUpdated(SocketUser oldU, SocketUser newU)
	{
		if (oldU.Username != newU.Username)
		{
			using var scope = _serviceProvider.CreateScope();

			var translator = scope.ServiceProvider.GetRequiredService<Translation>();

			foreach (var guild in newU.MutualGuilds)
			{
				await translator.SetLanguage(guild.Id);

				StringBuilder description = new();
				description.AppendLine(
					$"> **{translator.Get<BotTranslator>().User()}:** {newU.Username}#{newU.Discriminator} - {newU.Mention}");
				description.AppendLine($"> **{translator.Get<BotTranslator>().Id()}:** `{newU.Id}`");

				var embed = new EmbedBuilder()
					.WithTitle(translator.Get<GuildAuditTranslator>().UsernameUpdated())
					.WithDescription(description.ToString())
					.WithAuthor(newU);

				embed.AddField(
					translator.Get<GuildAuditTranslator>().Old(),
					oldU.Username,
					true
				);
				embed.AddField(
					translator.Get<GuildAuditTranslator>().New(),
					newU.Username,
					true
				);

				await SendEmbed(embed, guild.Id, GuildAuditEvent.UsernameUpdated);
			}
		}
	}

	public async Task HandleBanAdded(SocketUser user, SocketGuild guild)
	{
		using var scope = _serviceProvider.CreateScope();

		var translator = scope.ServiceProvider.GetRequiredService<Translation>();
		await translator.SetLanguage(guild.Id);

		StringBuilder description = new();
		description.AppendLine(
			$"> **{translator.Get<BotTranslator>().User()}:** {user.Username}#{user.Discriminator} - {user.Mention}");

		var embed = new EmbedBuilder()
			.WithTitle(translator.Get<GuildAuditTranslator>().UserBanned())
			.WithDescription(description.ToString())
			.WithAuthor(user)
			.WithFooter($"{translator.Get<BotTranslator>().UserId()}: {user.Id}");

		await SendEmbed(embed, guild.Id, GuildAuditEvent.BanAdded);
	}

	public async Task HandleBanRemoved(SocketUser user, SocketGuild guild)
	{
		using var scope = _serviceProvider.CreateScope();

		var translator = scope.ServiceProvider.GetRequiredService<Translation>();
		await translator.SetLanguage(guild.Id);

		StringBuilder description = new();
		description.AppendLine(
			$"> **{translator.Get<BotTranslator>().User()}:** {user.Username}#{user.Discriminator} - {user.Mention}");

		var embed = new EmbedBuilder()
			.WithTitle(translator.Get<GuildAuditTranslator>().UserUnbanned())
			.WithDescription(description.ToString())
			.WithAuthor(user)
			.WithFooter($"{translator.Get<BotTranslator>().UserId()}: {user.Id}");

		await SendEmbed(embed, guild.Id, GuildAuditEvent.BanRemoved);
	}

	public async Task HandleInviteCreated(SocketInvite invite)
	{
		using var scope = _serviceProvider.CreateScope();

		var translator = scope.ServiceProvider.GetRequiredService<Translation>();
		await translator.SetLanguage(invite.Guild.Id);

		EmbedBuilder embed = new();

		StringBuilder description = new();
		description.AppendLine($"> **{translator.Get<GuildAuditTranslator>().Url()}:** {invite}");

		if (invite.Inviter != null)
		{
			description.AppendLine(
				$"> **{translator.Get<BotTranslator>().User()}:** {invite.Inviter.Username}#{invite.Inviter.Discriminator} - {invite.Inviter.Mention}");
			embed.WithAuthor(invite.Inviter)
				.WithFooter($"{translator.Get<BotTranslator>().UserId()}: {invite.Inviter.Id}");
		}

		if (invite.Channel is ITextChannel tChannel)
			description.AppendLine(
				$"> **{translator.Get<GuildAuditTranslator>().TargetChannel()}:** {tChannel.Name} - {tChannel.Mention}");

		embed.WithTitle(translator.Get<GuildAuditTranslator>().InviteCreated())
			.WithDescription(description.ToString());

		if (invite.MaxUses != 0)
			embed.AddField(translator.Get<GuildAuditTranslator>().MaxUses(), $"`{invite.MaxUses}`", true);

		if (invite.CreatedAt != default && invite.MaxAge != default)
			embed.AddField(translator.Get<GuildAuditTranslator>().ExpirationDate(),
				invite.CreatedAt.AddSeconds(invite.MaxAge).DateTime.ToDiscordTs(), true);

		await SendEmbed(embed, invite.Guild.Id, GuildAuditEvent.InviteCreated);
	}

	public async Task HandleInviteDeleted(SocketGuildChannel channel, TrackedInvite invite)
	{
		using var scope = _serviceProvider.CreateScope();

		var translator = scope.ServiceProvider.GetRequiredService<Translation>();
		await translator.SetLanguage(channel.Guild.Id);

		EmbedBuilder embed = new();

		StringBuilder description = new();
		description.AppendLine($"> **{translator.Get<GuildAuditTranslator>().Url()}:** {invite}");

		var inviter = channel.Guild.GetUser(invite.CreatorId);

		if (inviter != null)
		{
			description.AppendLine(
				$"> **{translator.Get<BotTranslator>().User()}:** {inviter.Username}#{inviter.Discriminator} - {inviter.Mention}");
			embed.WithAuthor(inviter)
				.WithFooter($"{translator.Get<BotTranslator>().UserId()}: {inviter.Id}");
		}

		if (channel is ITextChannel tChannel)
			description.AppendLine(
				$"> **{translator.Get<GuildAuditTranslator>().TargetChannel()}:** {tChannel.Name} - {tChannel.Mention}");

		embed.WithTitle(translator.Get<GuildAuditTranslator>().InviteDeleted())
			.WithDescription(description.ToString());

		await SendEmbed(embed, channel.Guild.Id, GuildAuditEvent.InviteDeleted);
	}

	public async Task HandleUserJoined(SocketGuildUser user)
	{
		using var scope = _serviceProvider.CreateScope();

		var translator = scope.ServiceProvider.GetRequiredService<Translation>();
		await translator.SetLanguage(user.Guild.Id);

		StringBuilder description = new();
		description.AppendLine(
			$"> **{translator.Get<BotTranslator>().User()}:** {user.Username}#{user.Discriminator} - {user.Mention}");
		description.AppendLine($"> **{translator.Get<BotTranslator>().Id()}:** `{user.Id}`");
		description.AppendLine(
			$"> **{translator.Get<GuildAuditTranslator>().Registered()}:** {user.CreatedAt.DateTime.ToDiscordTs()}");

		var embed = new EmbedBuilder()
			.WithTitle(translator.Get<GuildAuditTranslator>().UserJoined())
			.WithDescription(description.ToString())
			.WithAuthor(user)
			.WithFooter($"{translator.Get<BotTranslator>().UserId()}: {user.Id}");

		await SendEmbed(embed, user.Guild.Id, GuildAuditEvent.UserJoined);
	}

	public async Task HandleUserRemoved(SocketGuild guild, SocketUser user)
	{
		using var scope = _serviceProvider.CreateScope();

		var translator = scope.ServiceProvider.GetRequiredService<Translation>();
		await translator.SetLanguage(guild.Id);

		StringBuilder description = new();
		description.AppendLine(
			$"> **{translator.Get<BotTranslator>().User()}:** {user.Username}#{user.Discriminator} - {user.Mention}");
		description.AppendLine($"> **{translator.Get<BotTranslator>().Id()}:** `{user.Id}`");
		description.AppendLine(
			$"> **{translator.Get<GuildAuditTranslator>().Registered()}:** {user.CreatedAt.DateTime.ToDiscordTs()}");

		var embed = new EmbedBuilder()
			.WithTitle(translator.Get<GuildAuditTranslator>().UserRemoved())
			.WithDescription(description.ToString())
			.WithAuthor(user)
			.WithFooter($"{translator.Get<BotTranslator>().UserId()}: {user.Id}");

		await SendEmbed(embed, guild.Id, GuildAuditEvent.UserRemoved);
	}

	public async Task HandleMessageDeleted(Cacheable<IMessage, ulong> messageCached,
		Cacheable<IMessageChannel, ulong> channel)
	{
		var message = await messageCached.GetOrDownloadAsync();

		if (message?.Channel is ITextChannel textChannel)
		{
			using var scope = _serviceProvider.CreateScope();

			var translator = scope.ServiceProvider.GetRequiredService<Translation>();
			await translator.SetLanguage(textChannel.Guild.Id);

			EmbedBuilder embed = new();

			StringBuilder description = new();
			description.AppendLine(
				$"> **{translator.Get<BotTranslator>().Channel()}:** {message.Channel.Name} - {textChannel.Mention}");
			description.AppendLine(
				$"> **{translator.Get<BotTranslator>().Id()}:** [{message.Id}]({message.GetJumpUrl()})");

			if (message.Author != null)
			{
				description.AppendLine(
					$"> **{translator.Get<BotTranslator>().Author()}:** {message.Author.Username}#{message.Author.Discriminator} - {message.Author.Mention}");
				embed.WithAuthor(message.Author)
					.WithFooter($"{translator.Get<BotTranslator>().UserId()}: {message.Author.Id}");
			}

			if (message.CreatedAt != default)
				description.AppendLine(
					$"> **{translator.Get<GuildAuditTranslator>().Created()}:** {message.CreatedAt.DateTime.ToDiscordTs()}");

			embed.WithTitle(translator.Get<GuildAuditTranslator>().MessageDeleted())
				.WithDescription(description.ToString());

			if (!string.IsNullOrEmpty(message.Content))
			{
				if (message.Content.Length > 1024)
				{
					for (var i = 0; i < 4; i++)
						if (message.Content.Length > i * 1024)
							embed.AddField(
								$"{translator.Get<GuildAuditTranslator>().Content()} [{i + 1}]",
								new string(message.Content.Skip(i * 1024).Take(1024).ToArray())
							);
				}
				else
					embed.AddField(translator.Get<GuildAuditTranslator>().Content(), message.Content);
			}

			if (message.Attachments.Count > 0)
			{
				StringBuilder attachmentInfo = new();

				var counter = 1;

				foreach (var attachment in message.Attachments.Take(5))
				{
					attachmentInfo.AppendLine(
						$"- [{counter}. {translator.Get<BotTranslator>().Attachment()}]({attachment.Url})");
					counter++;
				}

				if (message.Attachments.Count > 5)
					attachmentInfo.AppendLine(translator.Get<BotTranslator>().AndXMore(message.Attachments.Count - 5));

				embed.AddField(translator.Get<BotTranslator>().Attachments(), attachmentInfo.ToString());
			}

			await SendEmbed(embed, textChannel.Guild.Id, GuildAuditEvent.MessageDeleted);
		}
	}

	public async Task HandleMessageSent(IMessage message)
	{
		if (!message.Author.IsBot && !message.Author.IsWebhook)
			if (message.Channel is ITextChannel textChannel)
			{
				using var scope = _serviceProvider.CreateScope();

				var translator = scope.ServiceProvider.GetRequiredService<Translation>();
				await translator.SetLanguage(textChannel.Guild.Id);

				StringBuilder description = new();
				description.AppendLine(
					$"> **{translator.Get<BotTranslator>().Channel()}:** {textChannel.Name} - {textChannel.Mention}");
				description.AppendLine(
					$"> **{translator.Get<BotTranslator>().Id()}:** [{message.Id}]({message.GetJumpUrl()})");
				description.AppendLine(
					$"> **{translator.Get<BotTranslator>().Author()}:** {message.Author.Username}#{message.Author.Discriminator} - {message.Author.Mention}");

				var embed = new EmbedBuilder()
					.WithTitle(translator.Get<GuildAuditTranslator>().MessageSent())
					.WithDescription(description.ToString())
					.WithAuthor(message.Author)
					.WithFooter($"{translator.Get<BotTranslator>().UserId()}: {message.Author.Id}");

				if (!string.IsNullOrEmpty(message.Content))
				{
					if (message.Content.Length > 1024)
					{
						for (var i = 0; i < 4; i++)
							if (message.Content.Length > i * 1024)
								embed.AddField(
									$"{translator.Get<GuildAuditTranslator>().Content()} [{i + 1}]",
									new string(message.Content.Skip(i * 1024).Take(1024).ToArray())
								);
					}
					else
						embed.AddField(translator.Get<GuildAuditTranslator>().Content(), message.Content);
				}

				if (message.Attachments.Count > 0)
				{
					StringBuilder attachmentInfo = new();

					var counter = 1;

					foreach (var attachment in message.Attachments.Take(5))
					{
						attachmentInfo.AppendLine(
							$"- [{counter}. {translator.Get<BotTranslator>().Attachment()}]({attachment.Url})");
						counter++;
					}

					if (message.Attachments.Count > 5)
						attachmentInfo.AppendLine(translator.Get<BotTranslator>()
							.AndXMore(message.Attachments.Count - 5));

					embed.AddField(translator.Get<BotTranslator>().Attachments(), attachmentInfo.ToString());
				}

				await SendEmbed(embed, textChannel.Guild.Id, GuildAuditEvent.MessageSent);
			}
	}

	public async Task HandleMessageUpdated(Cacheable<IMessage, ulong> messageBefore, SocketMessage messageAfter,
		ISocketMessageChannel channel)
	{
		if (messageAfter.Author.Id == 0)
			return;

		if (!messageAfter.Author.IsBot && !messageAfter.Author.IsWebhook)
			if (channel is ITextChannel textChannel)
			{
				using var scope = _serviceProvider.CreateScope();

				var translator = scope.ServiceProvider.GetRequiredService<Translation>();
				await translator.SetLanguage(textChannel.Guild.Id);

				StringBuilder description = new();
				description.AppendLine(
					$"> **{translator.Get<BotTranslator>().Channel()}:** {textChannel.Name} - {textChannel.Mention}");
				description.AppendLine(
					$"> **{translator.Get<BotTranslator>().Id()}:** [{messageAfter.Id}]({messageAfter.GetJumpUrl()})");
				description.AppendLine(
					$"> **{translator.Get<BotTranslator>().Author()}:** {messageAfter.Author.Username}#{messageAfter.Author.Discriminator} - {messageAfter.Author.Mention}");
				description.AppendLine(
					$"> **{translator.Get<GuildAuditTranslator>().Created()}:** {messageAfter.CreatedAt.DateTime.ToDiscordTs()}");

				var embed = new EmbedBuilder()
					.WithTitle(translator.Get<GuildAuditTranslator>().MessageUpdated())
					.WithDescription(description.ToString())
					.WithAuthor(messageAfter.Author)
					.WithFooter($"{translator.Get<BotTranslator>().UserId()}: {messageAfter.Author.Id}");

				var before = await messageBefore.GetOrDownloadAsync();

				if (before == null)
					embed.AddField(translator.Get<GuildAuditTranslator>().Before(),
						translator.Get<GuildAuditTranslator>().InformationNotCached());
				else if (string.Equals(before.Content, messageAfter.Content) && before.Embeds.Count != messageAfter.Embeds.Count)
					return;
				else if (!string.Equals(before.Content, messageAfter.Content))
					if (!string.IsNullOrEmpty(before.Content))
						embed.AddField(translator.Get<GuildAuditTranslator>().Before(), before.Content.Truncate(1024));

				if (!string.IsNullOrEmpty(messageAfter.Content))
				{
					if (messageAfter.Content.Length > 1024)
					{
						for (var i = 0; i < 4; i++)
							if (messageAfter.Content.Length > i * 1024)
								embed.AddField(
									$"{translator.Get<GuildAuditTranslator>().New()} [{i + 1}]",
									new string(messageAfter.Content.Skip(i * 1024).Take(1024).ToArray())
								);
					}
					else
						embed.AddField(translator.Get<GuildAuditTranslator>().New(), messageAfter.Content);
				}

				if (messageAfter.Attachments.Count > 0)
				{
					StringBuilder attachmentInfo = new();

					var counter = 1;

					foreach (IAttachment attachment in messageAfter.Attachments.Take(5))
					{
						attachmentInfo.AppendLine(
							$"- [{counter}. {translator.Get<BotTranslator>().Attachment()}]({attachment.Url})");
						counter++;
					}

					if (messageAfter.Attachments.Count > 5)
						attachmentInfo.AppendLine(translator.Get<BotTranslator>()
							.AndXMore(messageAfter.Attachments.Count - 5));

					embed.AddField(translator.Get<BotTranslator>().Attachments(), attachmentInfo.ToString());
				}

				await SendEmbed(embed, textChannel.Guild.Id, GuildAuditEvent.MessageUpdated);
			}
	}

	public async Task HandleThreadCreated(SocketThreadChannel thread)
	{
		using var scope = _serviceProvider.CreateScope();

		var translator = scope.ServiceProvider.GetRequiredService<Translation>();
		await translator.SetLanguage(thread.Guild.Id);

		StringBuilder description = new();
		description.AppendLine($"> **{translator.Get<BotTranslator>().Channel()}:** {thread.Name} - {thread.Mention}");
		description.AppendLine(
			$"> **{translator.Get<GuildAuditTranslator>().Parent()}:** {thread.ParentChannel.Name} - {thread.ParentChannel.Mention}");
		description.AppendLine($"> **{translator.Get<GuildAuditTranslator>().Creator()}:** <@{thread.Owner}>");

		var embed = new EmbedBuilder()
			.WithTitle(translator.Get<GuildAuditTranslator>().ThreadCreated())
			.WithDescription(description.ToString())
			.WithFooter($"{translator.Get<BotTranslator>().ChannelId()}: {thread.Id}");

		await SendEmbed(embed, thread.Guild.Id, GuildAuditEvent.ThreadCreated);
	}
}