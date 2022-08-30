using Discord;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

	[NotMapped]
	public Offset2D TitleOffset
	{
		get => new Offset2D(TitleOffsetX, TitleOffsetY);
		set { TitleOffsetX = value.x; TitleOffsetY = value.y; }
	}
	[NotMapped]
	public Offset2D LevelOffset
	{
		get => new Offset2D(LevelOffsetX, LevelOffsetY);
		set { LevelOffsetX = value.x; LevelOffsetY = value.y; }
	}
	[NotMapped]
	public Offset2D PfpOffset
	{
		get => new Offset2D(PfpOffsetX, PfpOffsetY);
		set { PfpOffsetX = value.x; PfpOffsetY = value.y; }
	}

	[Key]
	public ulong Id { get; set; }
	public uint XpColor { get; set; } = 0xff70cefe;
	public uint OffColor { get; set; } = 0xffffffff;
	public uint LevelBgColor { get; set; } = 0xff202225;
	public uint TitleBgColor { get; set; } = 0xff202225;
	public string Background { get; set; } = "#555555";
	public int TitleOffsetX { get; set; } = 0;
	public int TitleOffsetY { get; set; } = 0;
	public int LevelOffsetX { get; set; } = 0;
	public int LevelOffsetY { get; set; } = 100;
	public int PfpOffsetX { get; set; } = 1000;
	public int PfpOffsetY { get; set; } = 100;
	public float PfpRadiusFactor { get; set; } = 0.9f;
	public RankcardFlags RankcardFlags { get; set; } = RankcardFlags.DisplayPfp | RankcardFlags.PfpBackground | RankcardFlags.ClipPfp | RankcardFlags.ShowHybrid;
}

public class Offset2D
{
	public int x;
	public int y;

	public Offset2D(int x = 0, int y = 0) {
		this.x = x;
		this.y = y;
	}

	public static implicit operator SixLabors.ImageSharp.Point(Offset2D o) => new(o.x, o.y);
	public static explicit operator Offset2D(SixLabors.ImageSharp.Point p) => new(p.X, p.Y);

	public static Offset2D operator +(Offset2D o, int n) => new(o.x + n, o.y + n);
	public static Offset2D operator -(Offset2D o, int n) => new(o.x - n, o.y - n);

	public override string ToString()
	{
		return $"({x}, {y})";
	}
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
