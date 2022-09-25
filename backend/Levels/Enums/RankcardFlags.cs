namespace Levels.Enums;

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
