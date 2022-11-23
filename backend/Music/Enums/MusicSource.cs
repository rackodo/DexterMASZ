using Discord.Interactions;

namespace Music.Enums;

public enum MusicSource
{
    [ChoiceDisplay("youtube")] YouTube,

    [ChoiceDisplay("soundcloud")] SoundCloud,

    [ChoiceDisplay("spotify")] Spotify,

    [ChoiceDisplay("deezer")] Deezer,

    [ChoiceDisplay("yandex-music"] YandexMusic,

    [ChoiceDisplay("raw")] None
}
