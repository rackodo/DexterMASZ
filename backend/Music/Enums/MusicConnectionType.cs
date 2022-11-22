using Discord.Interactions;

namespace Music.Enums;

public enum MusicConnectionType
{
    [ChoiceDisplay("normal")] Normal,

    [ChoiceDisplay("queued")] Queued
}
