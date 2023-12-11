using Lavalink4NET.Players.Vote;
using Lavalink4NET.Players;
using Music.Exceptions;
using Lavalink4NET;
using Music.Services;
using Discord;
using Lavalink4NET.DiscordNet;

namespace Music.Extensions;

public static class GetPlayer
{
    public static async ValueTask<VoteLavalinkPlayer> GetPlayerAsync(this IAudioService audio,
        IInteractionContext context, MusicService music, bool connectToVoiceChannel = false)
    {
        var channelBehavior = connectToVoiceChannel
        ? PlayerChannelBehavior.Join
            : PlayerChannelBehavior.None;

        var retrieveOptions = new PlayerRetrieveOptions(ChannelBehavior: channelBehavior);

        var result = await audio.Players
            .RetrieveAsync(context, PlayerFactory.Vote, retrieveOptions);

        if (!result.IsSuccess)
        {
            if (result.Status == PlayerRetrieveStatus.BotNotConnected && !connectToVoiceChannel)
            {
                await context.Interaction.FollowupAsync("Connecting to voice channel.");

                music.SetStartTimeAsCurrent(context.Guild.Id);
                return await audio.GetPlayerAsync(context, music, true);
            }

            var errorMessage = result.Status switch
            {
                PlayerRetrieveStatus.UserNotInVoiceChannel => throw new UserNotInVoiceChannel(),
                PlayerRetrieveStatus.BotNotConnected => "The bot is currently not connected.",
                _ => "Unknown error.",
            };

            await context.Interaction.FollowupAsync(errorMessage);
            return null;
        }

        return result.Player;
    }
}
