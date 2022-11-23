using Discord.Interactions;
using Discord.WebSocket;
using Lavalink4NET.Player;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("join", "Connect to your current voice channel")]
    public async Task ConnectMusic()
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureUserInVoiceAsync()) return;

        var player = Lavalink.GetPlayer(Context.Guild.Id);

        if (player != null)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Already connected.");

            return;
        }

        await Lavalink.JoinAsync<VoteLavalinkPlayer>(Context.Guild.Id,
            ((SocketGuildUser)Context.User).VoiceState!.Value.VoiceChannel.Id, true);

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Done establishing the connection.");
    }
}
