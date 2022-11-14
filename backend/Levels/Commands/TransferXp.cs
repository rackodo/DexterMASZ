using Bot.Abstractions;
using Bot.Attributes;
using Bot.Data;
using Bot.Enums;
using Bot.Services;
using Discord;
using Discord.Interactions;
using Levels.Data;
using Levels.Models;

namespace Levels.Commands;

public class TransferXp : Command<TransferXp>
{
    public GuildLevelConfigRepository? GuildLevelConfigRepository { get; set; }
    public GuildUserLevelRepository? GuildUserLevelRepository { get; set; }
    public UserRankcardConfigRepository? UserRankcardConfigRepository { get; set; }
    public SettingsRepository? SettingsRepository { get; set; }
    public DiscordRest? _client { get; set; }

    [Require(RequireCheck.GuildModerator)]
    [SlashCommand("transferxp", "Transfer experience from a source user to a target user.")]
    public async Task TransferXpCommand(
        [Summary("source", "The user to take the experience from.")]
        IUser source,
        [Summary("target", "The user to give the experience to.")]
        IUser target,
        [Summary("textXpCap", "The maximum amount of text experience to transfer.")]
        long textXpCap = -1,
        [Summary("voiceXpCap", "The maximum amount of voice experience to transfer.")]
        long voiceXpCap = -1
    )
    {
        var sourceLevel = await GuildUserLevelRepository!.GetOrCreateLevel(Context.Guild.Id, source.Id);
        var targetLevel = await GuildUserLevelRepository!.GetOrCreateLevel(Context.Guild.Id, target.Id);

        var textXp = sourceLevel.TextXp;
        var voiceXp = sourceLevel.VoiceXp;

        if (textXpCap >= 0 && textXp > textXpCap) textXp = textXpCap;
        if (voiceXpCap >= 0 && voiceXp > voiceXpCap) voiceXp = voiceXpCap;

        if (textXp == 0 && voiceXp == 0)
        {
            await RespondAsync($"No experience to transfer from {source.Mention} to {target.Mention}.",
                ephemeral: true);
            return;
        }

        targetLevel.TextXp += textXp;
        targetLevel.VoiceXp += voiceXp;
        sourceLevel.TextXp -= textXp;
        sourceLevel.VoiceXp -= voiceXp;

        var stringify = (GuildUserLevel l) => { return $"{l.UserId} ({l.TextXp}, {l.VoiceXp})"; };
        Console.WriteLine(
            $"Transferred ({textXp}, {voiceXp}) from {stringify(sourceLevel)} to {stringify(targetLevel)}.");

        try
        {
            await GuildUserLevelRepository.Save();
        }
        catch (Exception ex)
        {
            await RespondAsync("**An error occurred**: " + ex.Message, ephemeral: true);
            return;
        }

        var embed = new EmbedBuilder()
            .WithColor(Color.Blue)
            .WithTitle("XP Transfer Successful")
            .WithDescription(
                $"Transferred {textXp} text XP and {voiceXp} voice XP from {source.Mention} to {target.Mention}")
            .WithTimestamp(DateTime.UtcNow)
            .Build();

        await RespondAsync(embed: embed);
    }
}