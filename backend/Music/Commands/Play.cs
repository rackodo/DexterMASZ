using Bot.Attributes;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Lavalink4NET.Player;
using Lavalink4NET.Rest;
using Music.Enums;
using Music.Extensions;
using System.Text;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("play", "Add tracks to queue")]
    [BotChannel]
    public async Task AddMusic(
        [Summary("query", "Music query")] string query,
        [Summary("source", "Music source")] MusicSource source = MusicSource.YouTube)
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureUserInVoiceAsync()) return;
        if (!await EnsureClientInVoiceAsync()) return;

        if (!Enum.TryParse(source.ToString(), out SearchMode searchMode))
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Unable to get search mode");
            return;
        }

        var tracks = await Lavalink.GetTracksAsync(query, searchMode);

        var lavalinkTracks = tracks.ToList();

        if (!lavalinkTracks.Any())
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Unable to get the tracks");

            return;
        }

        var menuBuilder = new SelectMenuBuilder()
            .WithPlaceholder("Choose your tracks (max 10)")
            .WithCustomId("track-select-drop")
            .WithMaxValues(10)
            .AddOption("Wait I want to go back", "-1", "Use this in case you didn't find anything",
                Emoji.Parse(":x:"));

        var idx = 0;
        var options = new List<LavalinkTrack>();

        foreach (var track in lavalinkTracks.Take(24))
        {
            if (track.IsLiveStream) continue;
            var title = $"{track.Title} - {track.Author}";
            menuBuilder.AddOption(title.Length <= 100 ? title : string.Join("", title.Take(97)) + "...", $"{idx}",
                track.Uri?.AbsoluteUri);
            options.Add(track);
            ++idx;
        }

        var message = await Context.Interaction.ModifyOriginalResponseAsync(x =>
        {
            x.Content = "Choose your tracks";
            x.Components = new ComponentBuilder()
                .WithSelectMenu(menuBuilder)
                .Build();
        });

        var res = (SocketMessageComponent)await InteractionUtility.WaitForMessageComponentAsync(Context.Client,
            message,
            TimeSpan.FromMinutes(2));

        List<LavalinkTrack> tracksList = new();
        StringBuilder text = new();

        foreach (var value in res.Data.Values)
        {
            var iidx = Convert.ToInt32(value);

            if (iidx == -1)
            {
                await Context.Interaction.ModifyOriginalResponseAsync(x =>
                    x.Content = "No tracks added");

                text.Clear();
                break;
            }

            var track = options[iidx];

            if (track == null)
            {
                await Context.Interaction.ModifyOriginalResponseAsync(x =>
                    x.Content = "Error: Track was null!");
                return;
            }

            tracksList.Add(track);
            text.AppendLine($"{Format.Bold(Format.Sanitize(track!.Title))} by {Format.Bold(track.Author)}");
        }

        _player.Queue.AddRange(tracksList);

        await res.UpdateAsync(x =>
        {
            x.Content = "Tracks added";
            x.Components = new ComponentBuilder().Build();
            x.Embed = Context.User.CreateEmbedWithUserData()
                .WithAuthor("Added tracks to queue", Context.Client.CurrentUser.GetAvatarUrl())
                .WithDescription(string.IsNullOrWhiteSpace($"{text}") ? "Nothing" : $"{text}")
                .Build();
        });

        await PlayQueue();
    }
}
