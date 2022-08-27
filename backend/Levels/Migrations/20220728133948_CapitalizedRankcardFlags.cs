using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Levels.Migrations;

public partial class CapitalizedRankcardFlags : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.RenameColumn(
			name: "rankcardFlags",
			schema: "Levels",
			table: "UserRankcardConfigs",
			newName: "RankcardFlags");

		migrationBuilder.RenameColumn(
			name: "Voicexp",
			schema: "Levels",
			table: "GuildUserLevels",
			newName: "VoiceXp");

		migrationBuilder.RenameColumn(
			name: "Textxp",
			schema: "Levels",
			table: "GuildUserLevels",
			newName: "TextXp");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.RenameColumn(
			name: "RankcardFlags",
			schema: "Levels",
			table: "UserRankcardConfigs",
			newName: "rankcardFlags");

		migrationBuilder.RenameColumn(
			name: "VoiceXp",
			schema: "Levels",
			table: "GuildUserLevels",
			newName: "Voicexp");

		migrationBuilder.RenameColumn(
			name: "TextXp",
			schema: "Levels",
			table: "GuildUserLevels",
			newName: "Textxp");
	}
}
