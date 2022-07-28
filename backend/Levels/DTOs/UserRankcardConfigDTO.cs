using Levels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levels.DTOs;

public class UserRankcardConfigDTO
{
	public UserRankcardConfigDTO(ulong id, uint xpColor, uint offColor, uint levelBgColor, uint titleBgColor, string background, RankcardFlags rankcardFlags)
	{
		Id = id;
		XpColor = xpColor;
		OffColor = offColor;
		LevelBgColor = levelBgColor;
		TitleBgColor = titleBgColor;
		Background = background;
		RankcardFlags = rankcardFlags;
	}

	public ulong Id { get; set; }
	public uint XpColor { get; set; }
	public uint OffColor { get; set; }
	public uint LevelBgColor { get; set; }
	public uint TitleBgColor { get; set; }
	public string Background { get; set; }
	public RankcardFlags RankcardFlags { get; set; }
}
