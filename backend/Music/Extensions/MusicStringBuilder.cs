using Discord;
using Lavalink4NET.Tracks;
using System.Text;

namespace Music.Extensions;

public static class MusicStringBuilder
{
    public static void AddTrackToSb(this LavalinkTrack track, StringBuilder builder) =>
        builder.AppendLine($"{Format.Bold(Format.Sanitize(track!.Title))} by {Format.Bold(track.Author)}");
}
