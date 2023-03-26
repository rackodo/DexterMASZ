using Bot.Abstractions;
using Bot.Exceptions;
using Discord.Interactions;
using Discord.WebSocket;
using Lavalink4NET;
using Lavalink4NET.Player;
using Music.Extensions;
using Music.Services;

namespace Music.Commands;

[Group("music", "Music commands")]
public class MusicCommand<T> : Command<T>
{
    public VoteLavalinkPlayer Player;
    public IAudioService Lavalink { get; set; }
    public MusicService Music { get; set; }

    public override async Task BeforeExecuteAsync(ICommandInfo command)
    {
        await Context.Interaction.DeferAsync();

        await base.BeforeExecuteAsync(command);

        if (((SocketGuildUser)Context.Interaction.User).VoiceState == null)
            throw new UserNotInVoiceChannel();

        Player = await Lavalink.EnsureConnected(Context, Music);

        lock (Music.ChannelLocker)
        {
            if (!Music.GuildMusicChannel.ContainsKey(Context.Guild.Id))
                Music.GuildMusicChannel.Add(Context.Guild.Id, Context.Channel.Id);
            else
                Music.GuildMusicChannel[Context.Guild.Id] = Context.Channel.Id;
        }
    }
}
