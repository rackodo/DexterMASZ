using Bot.Data;
using Bot.Enums;
using Discord;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Extensions;

public static class EmbedCreator
{
	public static EmbedBuilder CreateColoredEmbed(Color color, Type type)
	{
		var ns = type.Namespace.Split('.');

		return new EmbedBuilder()
			.WithCurrentTimestamp()
			.WithColor(color)
			.WithFooter(ns.First() + ns.Last());
	}

	public static async Task<EmbedBuilder> CreateActionEmbed(RestAction action, IServiceProvider provider,
		IUser author = null)
	{
		var embed = new EmbedBuilder()
			.WithCurrentTimestamp()
			.WithColor(action switch
			{
				RestAction.Updated => Color.Orange,
				RestAction.Deleted => Color.Red,
				RestAction.Created => Color.Green,
				_ => Color.Blue
			});

		if (author != null)
			embed.WithAuthor(author);

		var config = await provider.GetRequiredService<SettingsRepository>().GetAppSettings();

		if (!string.IsNullOrEmpty(config.ServiceBaseUrl))
			embed.Url = config.ServiceBaseUrl;

		return embed;
	}
}