using Discord.Interactions;

namespace Music.Enums;

public enum MusicSource
{
    [ChoiceDisplay("soundcloud")] SoundCloud,

    [ChoiceDisplay("youtube")] YouTube,

    [ChoiceDisplay("raw")] None
}
