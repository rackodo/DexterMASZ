using Bot.Abstractions;
using Discord.Interactions;
using Discord.WebSocket;
using Lavalink4NET;
using Lavalink4NET.Player;
using Music.Exceptions;
using Music.Extensions;
using Music.Services;

namespace Music.Abstractions;

[Group("music", "Commands to run the inbuilt music player!")]
public class MusicCommand<T> : Command<T>
{
    public VoteLavalinkPlayer Player;
    public IAudioService Lavalink { get; set; }
    public MusicService Music { get; set; }

    public override async Task BeforeCommandExecute()
    {
        await Context.Interaction.DeferAsync();

        if (((SocketGuildUser)Context.Interaction.User).VoiceState == null)
            throw new UserNotInVoiceChannel();

        Player = await Lavalink.EnsureConnected(Context, Music);
        
        Music.SetCurrentChannelId(Context.Guild.Id, Context.Channel.Id);
    }
}
