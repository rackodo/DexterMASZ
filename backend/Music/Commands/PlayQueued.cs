using Bot.Attributes;
using Discord.Interactions;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("play-queued", "Play queued tracks")]
    [BotChannel]
    public async Task PlayQueueMusic()
    {
        await Context.Interaction.DeferAsync();

        if (!await PlayQueue())
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Could not play queue!");
    }

    public async Task<bool> PlayQueue()
    {
        if (!await EnsureUserInVoiceAsync()) return false;
        if (!await EnsureClientInVoiceAsync()) return false;
        if (!await EnsureQueueIsNotEmptyAsync()) return false;

        await _player.SkipAsync();

        Lavalink.TrackStarted += OnTrackStarted;
        Lavalink.TrackStuck += OnTrackStuck;
        Lavalink.TrackEnd += OnTrackEnd;
        Lavalink.TrackException += OnTrackException;

        return true;
    }
}
