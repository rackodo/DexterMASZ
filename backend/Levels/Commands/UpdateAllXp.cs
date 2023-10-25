using Bot.Abstractions;
using Bot.Attributes;
using Discord.Interactions;
using Bot.Enums;
using Levels.Data;
using Discord;
using Levels.Models;

namespace Levels.Commands;

public class UpdateAllXp : Command<UpdateAllXp>
{
    public GuildLevelConfigRepository GuildLevelConfigRepository { get; set; }
    public GuildUserLevelRepository GuildUserLevelRepository { get; set; }

    [SlashCommand("update-all-xp", "Updates all user's experience")]
    [Require(RequireCheck.GuildAdmin)]
    public async Task UpdateAllXpCommand()
    {
        var guild = Context.Guild;
        var userXps = GuildUserLevelRepository.GetAllLevelsInGuild(guild.Id);
        var guildConfig = await GuildLevelConfigRepository!.GetOrCreateConfig(guild.Id);

        var roles = guildConfig.Levels.ToDictionary(
            x => x.Key,
            x => x.Value.Where(x => guild.GetRole(x) != null).ToArray()
        );

        var users = await guild.GetUsersAsync().FlattenAsync();
        var count = 0;
        var totalCount = users.Count();

        await RespondInteraction($"Updating all user roles in {guild.Name}!");

        foreach (var user in users)
        {
            var xp = userXps.FirstOrDefault(x => x.UserId == user.Id);
            var level = 0;

            if (xp != null)
            {
                var calcLevel = new CalculatedGuildUserLevel(xp, guildConfig);
                level = calcLevel.Total.Level;
            }

            var addedRoles = roles.Where(x => x.Key <= level)
                .SelectMany(x => x.Value)
                .Where(x => !user.RoleIds.Contains(x));

            var removedRoles = roles.Where(x => x.Key > level)
                .SelectMany(x => x.Value)
                .Where(x => user.RoleIds.Contains(x));

            if (addedRoles.Any())
                await user.AddRolesAsync(addedRoles);

            if (removedRoles.Any())
                await user.RemoveRolesAsync(removedRoles);

            count++;

            if (count % 50 == 0)
            {
                var progress = Math.Floor((double)count / totalCount * 100);
                await RespondInteraction($"Progress {count}/{totalCount} ({progress}%)");
            }
        }

        await RespondInteraction($"Successfully updated all users in {guild.Name}!");
    }
}
