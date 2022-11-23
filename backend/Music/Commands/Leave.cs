using Bot.Attributes;
using Discord.Interactions;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("leave", "Leave current voice channel")]
    [BotChannel]
    public async Task DisconnectMusic()
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureClientInVoiceAsync()) return;

        Lavalink.TrackStarted -= OnTrackStarted;
        Lavalink.TrackStuck -= OnTrackStuck;
        Lavalink.TrackEnd -= OnTrackEnd;
        Lavalink.TrackException -= OnTrackException;

        await _player.DisconnectAsync();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Left the voice channel");
    }
}
