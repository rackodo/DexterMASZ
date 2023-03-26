using Bot.Abstractions;
using Discord.Interactions;
using Discord.WebSocket;
using Lavalink4NET;
using Lavalink4NET.Player;
using Music.Exceptions;
using Music.Extensions;
using Music.Services;

namespace Music.Abstractions;

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

        var playerConnected = await Lavalink.EnsureConnected(Context, Music);

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = playerConnected.Item2);

        Player = playerConnected.Item1;

        Music.SetCurrentChannelId(Context.Guild.Id, Context.Channel.Id);
    }
}
