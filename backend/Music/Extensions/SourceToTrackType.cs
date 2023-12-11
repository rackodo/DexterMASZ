using Lavalink4NET.Rest.Entities.Tracks;
using Music.Enums;

namespace Music.Extensions;

public static class SourceToTrackType
{
    public static TrackSearchMode GetSearchMode(this MusicSource source) => source switch
    {
        MusicSource.Default => TrackSearchMode.None,
        MusicSource.YouTube => TrackSearchMode.YouTube,
        MusicSource.YouTubeMusic => TrackSearchMode.YouTubeMusic,
        MusicSource.SoundCloud => TrackSearchMode.SoundCloud,
        MusicSource.Spotify => TrackSearchMode.Spotify,
        MusicSource.Deezer => TrackSearchMode.Deezer,
        MusicSource.YandexMusic => TrackSearchMode.YandexMusic,
        _ => throw new NotImplementedException()
    };
}
