using Discord;
using System.ComponentModel.DataAnnotations;

namespace Levels.Models;

public class UserRankcardConfig
{
	public UserRankcardConfig() { }

	public UserRankcardConfig(IUser user)
	{
		Id = user.Id;
	}

	public UserRankcardConfig(ulong userId)
	{
		Id = userId;
	}

	[Key]
	public ulong Id { get; set; }
	public uint XpColor { get; set; } = 0xff70cefe;
	public uint OffColor { get; set; } = 0xffffffff;
	public uint LevelBgColor { get; set; } = 0xff202225;
	public uint TitleBgColor { get; set; } = 0xff202225;
	public string Background { get; set; } = "#555555";
	public RankcardFlags RankcardFlags { get; set; } = RankcardFlags.DisplayPfp | RankcardFlags.PfpBackground | RankcardFlags.ClipPfp | RankcardFlags.ShowHybrid;
}

[Flags]
public enum RankcardFlags
{
	None = 0,
	PfpBackground = 1 << 0,
	ClipPfp = 1 << 1,
	DisplayPfp = 1 << 2,
	ShowHybrid = 1 << 3,
	InsetMainXP = 1 << 4
}
