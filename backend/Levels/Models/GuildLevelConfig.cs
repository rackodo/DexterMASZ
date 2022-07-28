using Discord;
using System.ComponentModel.DataAnnotations;

namespace Levels.Models;

public class GuildLevelConfig
{
	public GuildLevelConfig() { }

	public GuildLevelConfig(IGuild guild)
	{
		Id = guild.Id;
	}

	public GuildLevelConfig(ulong guildId)
	{
		Id = guildId;
	}

	[Key]
	public ulong Id { get; set; }
	public float[] Coefficients { get; set; } = { 0f, 75.83333f, 22.5f, 1.66667f };
	public int XPInterval { get; set; } = 60;
	public int MinimumTextXPGiven { get; set; }
	public int MaximumTextXPGiven { get; set; }
	public int MinimumVoiceXPGiven { get; set; }
	public int MaximumVoiceXPGiven { get; set; }
	public string LevelUpTemplate { get; set; } = "{USER} leveled up to level {LEVEL}!";
	public ulong VoiceLevelUpChannel { get; set; }
	public ulong TextLevelUpChannel { get; set; }

	public ulong[] DisabledXPChannels { get; set; } = { };
	public bool HandleRoles { get; set; } = false;
	public bool SendTextLevelUps { get; set; } = true;
	public bool SendVoiceLevelUps { get; set; } = true;
	public bool VoiceXPCountMutedMembers { get; set; } = true;
	public int VoiceXPRequiredMembers { get; set; }

	public ulong NicknameDisabledRole { get; set; }
	public ulong NicknameDisabledReplacement { get; set; }

	public int RankcardImageSizeLimit { get; set; } = 0x400000;
	public int RankcardImageRequiredLevel { get; set; }

	public Dictionary<int, ulong[]> Levels { get; set; } = new();
	public Dictionary<int, string> LevelUpMessageOverrides { get; set; } = new();
}
