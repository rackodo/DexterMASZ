using Bot.Attributes;
using Bot.Extensions;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Lavalink4NET.Player;
using Lavalink4NET.Rest;
using Music.Abstractions;
using Music.Enums;
using Music.Extensions;
using System.Text;

namespace Music.Commands;

public class PlayCommand : MusicCommand<PlayCommand>
{
    [SlashCommand("play", "Add tracks to queue")]
    [BotChannel]
    public async Task Play(
        [Summary("query", "Music query")] string query,
        [Summary("source", "Music source")] MusicSource source)
    {
        var searchMode = SearchMode.None;
        StringBuilder tInfoSb = new();

        if (Uri.IsWellFormedUriString(query, UriKind.Absolute))
        {
            var track = await Lavalink.GetTrackAsync(query);

            if (track == null)
            {
                await Context.Interaction.ModifyOriginalResponseAsync(x =>
                    x.Content = $"Could not find track for {query}!");
                return;
            }

            Player.Queue.Add(track);
            track.AddTrackToSb(tInfoSb);
        }
        else
        {
            switch (source)
            {
                case MusicSource.SoundCloud:
                    searchMode = SearchMode.SoundCloud;
                    break;
                case MusicSource.YouTube:
                    searchMode = SearchMode.YouTube;
                    break;
                case MusicSource.Spotify:
                    query = $"spsearch:{query}";
                    break;
                case MusicSource.Deezer:
                    query = $"dzsearch:{query}";
                    break;
                case MusicSource.YandexMusic:
                    query = $"ymsearch:{query}";
                    break;
                case MusicSource.None:
                    break;
                default:
                    await Context.Interaction.ModifyOriginalResponseAsync(x =>
                        x.Content = "Unable to get search mode");
                    return;
            }

            if (query != null)
            {
                var tracks = await Lavalink.GetTracksAsync(query, searchMode);

                var lavalinkTracks = tracks.ToList();

                if (!lavalinkTracks.Any())
                {
                    await Context.Interaction.ModifyOriginalResponseAsync(x =>
                        x.Content =
                            "Unable to get tracks. If this was a link to a stream or playlist, please use `/music play-stream` or `play-playlist`.");

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
                    menuBuilder.AddOption(title.CropText(100), $"{idx}",
                        track.Uri?.AbsoluteUri.CropText(100));
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

                if (res != null)
                    await res.UpdateAsync(x => x.Components = new ComponentBuilder().Build());

                if (res?.Data?.Values != null)
                {
                    foreach (var value in res.Data.Values)
                    {
                        var newIdx = Convert.ToInt32(value);

                        if (newIdx == -1)
                        {
                            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                                x.Content = "No tracks added");

                            tInfoSb.Clear();
                            break;
                        }

                        var track = options[newIdx];

                        if (track == null)
                        {
                            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                                x.Content = $"Could not find track for {query}, index {newIdx}!");
                            return;
                        }

                        tracksList.Add(track);
                        track.AddTrackToSb(tInfoSb);
                    }

                    Player.Queue.AddRange(tracksList);
                }
                else
                {
                    await Context.Interaction.ModifyOriginalResponseAsync(x => x.Content = "Timed out!");
                    return;
                }
            }
        }

        var embed =
            Context.User.CreateEmbedWithUserData()
                .WithAuthor("Added tracks to queue", Context.Client.CurrentUser.GetAvatarUrl())
                .WithDescription((string.IsNullOrWhiteSpace($"{tInfoSb}") ? "Nothing\n" : $"{tInfoSb}") +
                                 $"Music from {Format.Bold(Enum.GetName(source))}.")
                .Build();

        if (Player.State != PlayerState.Playing)
            await Player.ResumeAsync();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
        {
            x.Embed = embed;
            x.Content = string.Empty;
        });
    }
}
