using Bot.Abstractions;
using Bot.Data;
using Bot.Enums;
using Bot.Exceptions;
using Bot.Services;
using Discord;
using Discord.Interactions;
using Levels.Data;
using Levels.Models;
using Levels.Services;

namespace Levels.Commands;

public class UpdateRoles : Command<UpdateRoles>
{
    public GuildLevelConfigRepository? GuildLevelConfigRepository { get; set; }
    public GuildUserLevelRepository? GuildUserLevelRepository { get; set; }
    public UserRankcardConfigRepository? UserRankcardConfigRepository { get; set; }
    public SettingsRepository? SettingsRepository { get; set; }
    public LevelingService? LevelingService { get; set; }
    public DiscordRest? Client { get; set; }

    [SlashCommand("updateroles", "Update a user's roles to match their level.", runMode: RunMode.Async)]
    public async Task RankCommand(
        [Summary("user", "User to update roles for. Defaults to oneself.")]
        IGuildUser? user = null
    )
    {
        user ??= Context.Guild.GetUser(Context.User.Id);

        if (user is null)
        {
            await RespondAsync("Unable to find guild user. Are you using this command in a registered guild?",
                ephemeral: true);
            return;
        }

        if (user != Context.User)
            try
            {
                await Identity.RequirePermission(DiscordPermission.Moderator, Context.Guild.Id);
            }
            catch (UnauthorizedException)
            {
                await RespondAsync(
                    "You must be staff to use this command on a different user. The target user can use this command without a given user to update their own roles.",
                    ephemeral: true);
                return;
            }
            catch (Exception e)
            {
                await RespondAsync("An unexpected error occurred while processing the command: " + e.Message,
                    ephemeral: true);
                return;
            }

        await DeferAsync(true);
        var level = await GuildUserLevelRepository!.GetOrCreateLevel(Context.Guild.Id, user.Id);
        var guildlevelconfig = await GuildLevelConfigRepository!.GetOrCreateConfig(Context.Guild.Id);
        var calclevel = new CalculatedGuildUserLevel(level, guildlevelconfig);

        var totalLevel = calclevel.Total.Level;

        var result = await LevelingService.HandleLevelRoles(level, totalLevel, user, GuildLevelConfigRepository);

        var embed = new EmbedBuilder()
            .WithTitle("Role update")
            .WithCurrentTimestamp();

        if (!result.IsErrored)
            embed
                .WithColor(Color.Green)
                .WithDescription($"Successfully updated {user.Mention}'s roles (level {level})")
                .AddField("Added Roles", result.AddedRoles)
                .AddField("Removed Roles", result.RemovedRoles);
        else
            embed
                .WithColor(Color.Red)
                .WithDescription($"Unable to update {user.Mention}'s roles (level {level})")
                .AddField("Error", result.Error);

        await FollowupAsync(embed: embed.Build());
    }
}
