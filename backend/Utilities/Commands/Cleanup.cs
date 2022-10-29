using Bot.Abstractions;
using Bot.Attributes;
using Bot.Enums;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using Discord.Net;
using System.Net;
using Utilities.Enums;
using Utilities.Translators;

namespace Utilities.Commands;

public class Cleanup : Command<Cleanup>
{
	[Require(RequireCheck.GuildModerator)]
	[SlashCommand("cleanup", "Cleanup specific data from the server and/or channel.")]
	public async Task CleanupCommand(
		[Summary("mode", "The data you wish to clean up.")]
		CleanupMode cleanupMode,
		[Summary("user", "The user to filter the cleanup to.")]
		IUser filterUser = null,
		[Summary("channel", "The channel to delete in, defaults to current.")]
		ITextChannel channel = null,
		[Summary("count", "The amount of messages to delete, defaults to 100.")]
		long count = 100)
	{
		if (cleanupMode == CleanupMode.Messages && filterUser == null)
		{
			await Context.Interaction.RespondAsync("I can't clean up all these messages! Please provide a filter...", ephemeral: true);
			return;
		}

		if (channel is null)
			if (Context.Channel is ITextChannel txtChannel)
			{
				channel = txtChannel;
			}
			else
			{
				await Context.Interaction.RespondAsync(Translator.Get<BotTranslator>().OnlyTextChannel(),
					ephemeral: true);
				return;
			}

		await Context.Interaction.DeferAsync(ephemeral: !GuildConfig.StaffChannels.Contains(Context.Channel.Id));

		if (count > 1000)
			count = 1000;

		var func = cleanupMode switch
		{
			CleanupMode.Bots => IsFromBot,
			CleanupMode.Attachments => HasAttachment,
			_ => new Func<IMessage, bool>(_ => true)
		};

		int deleted;

		try
		{
			deleted = await IterateAndDeleteChannels(channel, (int)count, func, Context.User, filterUser);
		}
		catch (HttpException ex)
		{
			if (ex.HttpCode == HttpStatusCode.Forbidden)
				await Context.Interaction.ModifyOriginalResponseAsync(msg =>
					msg.Content = Translator.Get<BotTranslator>().CannotViewOrDeleteInChannel());
			else if (ex.HttpCode == HttpStatusCode.Forbidden)
				await Context.Interaction.ModifyOriginalResponseAsync(msg =>
					msg.Content = Translator.Get<BotTranslator>().CannotFindChannel());

			return;
		}

		await Context.Interaction.ModifyOriginalResponseAsync(msg =>
			msg.Content = Translator.Get<UtilityTranslator>().DeletedMessages(deleted, channel));
	}

	private static bool HasAttachment(IMessage m)
	{
		return m.Attachments.Count > 0;
	}

	private static bool IsFromBot(IMessage m)
	{
		return m.Author.IsBot;
	}

	private static async Task<int> IterateAndDeleteChannels(ITextChannel channel, int limit,
		Func<IMessage, bool> predicate, IUser currentActor, IUser filterUser = null)
	{
		var latestMessage = await channel.GetMessagesAsync(1).FirstOrDefaultAsync();

		if (latestMessage is null)
			return 0;
		else if (latestMessage.FirstOrDefault() is null)
			return 0;

		var lastId = latestMessage.First().Id;
		var deleted = 0;

		while (limit > 0)
		{
			var messages = channel.GetMessagesAsync(lastId, Direction.Before, Math.Min(limit, 100));
			var toDelete = new List<IMessage>();

			if (await messages.CountAsync() == 0)
			{
				break;
			}
			foreach (var message in await messages.FlattenAsync())
			{
				lastId = message.Id;
				limit--;
				if (filterUser != null && message.Author.Id != filterUser.Id)
				{
					continue;
				}
				if (predicate(message))
				{
					deleted++;
					if (message.CreatedAt.UtcDateTime.AddDays(14) > DateTime.UtcNow)
					{
						toDelete.Add(message);
					}
					else
					{
						await message.DeleteAsync();
					}
				}
			}
			if (toDelete.Count >= 2)
			{
				RequestOptions options = new()
				{
					AuditLogReason = $"Bulkdelete by {currentActor.Username}#{currentActor.Discriminator} ({currentActor.Id})."
				};

				await channel.DeleteMessagesAsync(toDelete, options);
				toDelete.Clear();
			}
			else if (toDelete.Count > 0)
			{
				await toDelete.First().DeleteAsync();
				toDelete.Clear();
			}
		}
		return deleted;
	}
}