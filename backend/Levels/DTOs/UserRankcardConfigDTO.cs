using Levels.Enums;
using Levels.Models;

namespace Levels.DTOs;

public class UserRankcardConfigDto(ulong id, uint xpColor, uint offColor, uint levelBgColor, uint titleBgColor,
    string background, float pfpRadiusFactor, Offset2D titleOffset, Offset2D levelOffset, Offset2D pfpOffset,
    RankcardFlags rankcardFlags)
{
    public ulong Id { get; set; } = id;
    public uint XpColor { get; set; } = xpColor;
    public uint OffColor { get; set; } = offColor;
    public uint LevelBgColor { get; set; } = levelBgColor;
    public uint TitleBgColor { get; set; } = titleBgColor;
    public string Background { get; set; } = background;
    public Offset2D TitleOffset { get; set; } = titleOffset;
    public Offset2D LevelOffset { get; set; } = levelOffset;
    public Offset2D PfpOffset { get; set; } = pfpOffset;
    public float PfpRadiusFactor { get; set; } = pfpRadiusFactor;
    public RankcardFlags RankcardFlags { get; set; } = rankcardFlags;
}
