using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Levels.Migrations;

public partial class AddedOffsets : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<string>(
			name: "Background",
			schema: "Levels",
			table: "UserRankcardConfigs",
			type: "longtext",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "longtext")
			.Annotation("MySql:CharSet", "utf8mb4")
			.OldAnnotation("MySql:CharSet", "utf8mb4");

		migrationBuilder.AddColumn<float>(
			name: "LevelOffsetX",
			schema: "Levels",
			table: "UserRankcardConfigs",
			type: "float",
			nullable: false,
			defaultValue: 0f);

		migrationBuilder.AddColumn<float>(
			name: "LevelOffsetY",
			schema: "Levels",
			table: "UserRankcardConfigs",
			type: "float",
			nullable: false,
			defaultValue: 0f);

		migrationBuilder.AddColumn<float>(
			name: "PfpOffsetX",
			schema: "Levels",
			table: "UserRankcardConfigs",
			type: "float",
			nullable: false,
			defaultValue: 0f);

		migrationBuilder.AddColumn<float>(
			name: "PfpOffsetY",
			schema: "Levels",
			table: "UserRankcardConfigs",
			type: "float",
			nullable: false,
			defaultValue: 0f);

		migrationBuilder.AddColumn<float>(
			name: "PfpRadiusFactor",
			schema: "Levels",
			table: "UserRankcardConfigs",
			type: "float",
			nullable: false,
			defaultValue: 0f);

		migrationBuilder.AddColumn<float>(
			name: "TitleOffsetX",
			schema: "Levels",
			table: "UserRankcardConfigs",
			type: "float",
			nullable: false,
			defaultValue: 0f);

		migrationBuilder.AddColumn<float>(
			name: "TitleOffsetY",
			schema: "Levels",
			table: "UserRankcardConfigs",
			type: "float",
			nullable: false,
			defaultValue: 0f);

		migrationBuilder.AlterColumn<string>(
			name: "Levels",
			schema: "Levels",
			table: "GuildLevelConfigs",
			type: "longtext",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "longtext")
			.Annotation("MySql:CharSet", "utf8mb4")
			.OldAnnotation("MySql:CharSet", "utf8mb4");

		migrationBuilder.AlterColumn<string>(
			name: "LevelUpTemplate",
			schema: "Levels",
			table: "GuildLevelConfigs",
			type: "longtext",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "longtext")
			.Annotation("MySql:CharSet", "utf8mb4")
			.OldAnnotation("MySql:CharSet", "utf8mb4");

		migrationBuilder.AlterColumn<string>(
			name: "LevelUpMessageOverrides",
			schema: "Levels",
			table: "GuildLevelConfigs",
			type: "longtext",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "longtext")
			.Annotation("MySql:CharSet", "utf8mb4")
			.OldAnnotation("MySql:CharSet", "utf8mb4");

		migrationBuilder.AlterColumn<string>(
			name: "DisabledXpChannels",
			schema: "Levels",
			table: "GuildLevelConfigs",
			type: "longtext",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "longtext")
			.Annotation("MySql:CharSet", "utf8mb4")
			.OldAnnotation("MySql:CharSet", "utf8mb4");

		migrationBuilder.AlterColumn<string>(
			name: "Coefficients",
			schema: "Levels",
			table: "GuildLevelConfigs",
			type: "longtext",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "longtext")
			.Annotation("MySql:CharSet", "utf8mb4")
			.OldAnnotation("MySql:CharSet", "utf8mb4");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "LevelOffsetX",
			schema: "Levels",
			table: "UserRankcardConfigs");

		migrationBuilder.DropColumn(
			name: "LevelOffsetY",
			schema: "Levels",
			table: "UserRankcardConfigs");

		migrationBuilder.DropColumn(
			name: "PfpOffsetX",
			schema: "Levels",
			table: "UserRankcardConfigs");

		migrationBuilder.DropColumn(
			name: "PfpOffsetY",
			schema: "Levels",
			table: "UserRankcardConfigs");

		migrationBuilder.DropColumn(
			name: "PfpRadiusFactor",
			schema: "Levels",
			table: "UserRankcardConfigs");

		migrationBuilder.DropColumn(
			name: "TitleOffsetX",
			schema: "Levels",
			table: "UserRankcardConfigs");

		migrationBuilder.DropColumn(
			name: "TitleOffsetY",
			schema: "Levels",
			table: "UserRankcardConfigs");

		migrationBuilder.UpdateData(
			schema: "Levels",
			table: "UserRankcardConfigs",
			keyColumn: "Background",
			keyValue: null,
			column: "Background",
			value: "");

		migrationBuilder.AlterColumn<string>(
			name: "Background",
			schema: "Levels",
			table: "UserRankcardConfigs",
			type: "longtext",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "longtext",
			oldNullable: true)
			.Annotation("MySql:CharSet", "utf8mb4")
			.OldAnnotation("MySql:CharSet", "utf8mb4");

		migrationBuilder.UpdateData(
			schema: "Levels",
			table: "GuildLevelConfigs",
			keyColumn: "Levels",
			keyValue: null,
			column: "Levels",
			value: "");

		migrationBuilder.AlterColumn<string>(
			name: "Levels",
			schema: "Levels",
			table: "GuildLevelConfigs",
			type: "longtext",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "longtext",
			oldNullable: true)
			.Annotation("MySql:CharSet", "utf8mb4")
			.OldAnnotation("MySql:CharSet", "utf8mb4");

		migrationBuilder.UpdateData(
			schema: "Levels",
			table: "GuildLevelConfigs",
			keyColumn: "LevelUpTemplate",
			keyValue: null,
			column: "LevelUpTemplate",
			value: "");

		migrationBuilder.AlterColumn<string>(
			name: "LevelUpTemplate",
			schema: "Levels",
			table: "GuildLevelConfigs",
			type: "longtext",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "longtext",
			oldNullable: true)
			.Annotation("MySql:CharSet", "utf8mb4")
			.OldAnnotation("MySql:CharSet", "utf8mb4");

		migrationBuilder.UpdateData(
			schema: "Levels",
			table: "GuildLevelConfigs",
			keyColumn: "LevelUpMessageOverrides",
			keyValue: null,
			column: "LevelUpMessageOverrides",
			value: "");

		migrationBuilder.AlterColumn<string>(
			name: "LevelUpMessageOverrides",
			schema: "Levels",
			table: "GuildLevelConfigs",
			type: "longtext",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "longtext",
			oldNullable: true)
			.Annotation("MySql:CharSet", "utf8mb4")
			.OldAnnotation("MySql:CharSet", "utf8mb4");

		migrationBuilder.UpdateData(
			schema: "Levels",
			table: "GuildLevelConfigs",
			keyColumn: "DisabledXpChannels",
			keyValue: null,
			column: "DisabledXpChannels",
			value: "");

		migrationBuilder.AlterColumn<string>(
			name: "DisabledXpChannels",
			schema: "Levels",
			table: "GuildLevelConfigs",
			type: "longtext",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "longtext",
			oldNullable: true)
			.Annotation("MySql:CharSet", "utf8mb4")
			.OldAnnotation("MySql:CharSet", "utf8mb4");

		migrationBuilder.UpdateData(
			schema: "Levels",
			table: "GuildLevelConfigs",
			keyColumn: "Coefficients",
			keyValue: null,
			column: "Coefficients",
			value: "");

		migrationBuilder.AlterColumn<string>(
			name: "Coefficients",
			schema: "Levels",
			table: "GuildLevelConfigs",
			type: "longtext",
			nullable: false,
			oldClrType: typeof(string),
			oldType: "longtext",
			oldNullable: true)
			.Annotation("MySql:CharSet", "utf8mb4")
			.OldAnnotation("MySql:CharSet", "utf8mb4");
	}
}
