using Levels.Enums;
using Levels.Models;

namespace Levels.DTOs;

public class UserRankcardConfigDTO
{
	public ulong Id { get; set; }
	public uint XpColor { get; set; }
	public uint OffColor { get; set; }
	public uint LevelBgColor { get; set; }
	public uint TitleBgColor { get; set; }
	public string Background { get; set; }
	public Offset2D TitleOffset { get; set; }
	public Offset2D LevelOffset { get; set; }
	public Offset2D PfpOffset { get; set; }
	public float PfpRadiusFactor { get; set; }
	public RankcardFlags RankcardFlags { get; set; }

	public UserRankcardConfigDTO(ulong id, uint xpColor, uint offColor, uint levelBgColor, uint titleBgColor, string background, float pfpRadiusFactor, Offset2D titleOffset, Offset2D levelOffset, Offset2D pfpOffset, RankcardFlags rankcardFlags)
	{
		Id = id;
		XpColor = xpColor;
		OffColor = offColor;
		LevelBgColor = levelBgColor;
		TitleBgColor = titleBgColor;
		Background = background;
		PfpRadiusFactor = pfpRadiusFactor;
		TitleOffset = titleOffset;
		LevelOffset = levelOffset;
		PfpOffset = pfpOffset;
		RankcardFlags = rankcardFlags;
	}
}
