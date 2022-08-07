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
	public int XpInterval { get; set; } = 60;
	public int MinimumTextXpGiven { get; set; }
	public int MaximumTextXpGiven { get; set; }
	public int MinimumVoiceXpGiven { get; set; }
	public int MaximumVoiceXpGiven { get; set; }
	public string LevelUpTemplate { get; set; } = "{USER} leveled up to level {LEVEL}!";
	public ulong VoiceLevelUpChannel { get; set; }
	public ulong TextLevelUpChannel { get; set; }

	public ulong[] DisabledXpChannels { get; set; } = { };
	public bool HandleRoles { get; set; } = false;
	public bool SendTextLevelUps { get; set; } = true;
	public bool SendVoiceLevelUps { get; set; } = true;
	public bool VoiceXpCountMutedMembers { get; set; } = true;
	public int VoiceXpRequiredMembers { get; set; } = 3;

	public ulong NicknameDisabledRole { get; set; }
	public ulong NicknameDisabledReplacement { get; set; }

	public Dictionary<int, ulong[]> Levels { get; set; } = new();
	public Dictionary<int, string> LevelUpMessageOverrides { get; set; } = new();
}
