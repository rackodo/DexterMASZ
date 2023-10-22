using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Levels.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "Levels");

        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "GuildLevelConfigs",
            schema: "Levels",
            columns: table => new
            {
                Id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Coefficients = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                XpInterval = table.Column<int>(type: "int", nullable: false),
                MinimumTextXpGiven = table.Column<int>(type: "int", nullable: false),
                MaximumTextXpGiven = table.Column<int>(type: "int", nullable: false),
                MinimumVoiceXpGiven = table.Column<int>(type: "int", nullable: false),
                MaximumVoiceXpGiven = table.Column<int>(type: "int", nullable: false),
                LevelUpTemplate = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                VoiceLevelUpChannel = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                TextLevelUpChannel = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                DisabledXpChannels = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                HandleRoles = table.Column<bool>(type: "tinyint(1)", nullable: false),
                SendTextLevelUps = table.Column<bool>(type: "tinyint(1)", nullable: false),
                SendVoiceLevelUps = table.Column<bool>(type: "tinyint(1)", nullable: false),
                VoiceXpCountMutedMembers = table.Column<bool>(type: "tinyint(1)", nullable: false),
                VoiceXpRequiredMembers = table.Column<int>(type: "int", nullable: false),
                NicknameDisabledRole = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                NicknameDisabledReplacement = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                Levels = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                LevelUpMessageOverrides = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table => table.PrimaryKey("PK_GuildLevelConfigs", x => x.Id))
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "GuildUserLevels",
            schema: "Levels",
            columns: table => new
            {
                UserId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                GuildId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                TextXp = table.Column<long>(type: "bigint", nullable: false),
                VoiceXp = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_GuildUserLevels", x => new { x.GuildId, x.UserId }))
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "UserRankcardConfigs",
            schema: "Levels",
            columns: table => new
            {
                Id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                XpColor = table.Column<uint>(type: "int unsigned", nullable: false),
                OffColor = table.Column<uint>(type: "int unsigned", nullable: false),
                LevelBgColor = table.Column<uint>(type: "int unsigned", nullable: false),
                TitleBgColor = table.Column<uint>(type: "int unsigned", nullable: false),
                Background = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                TitleOffsetX = table.Column<int>(type: "int", nullable: false),
                TitleOffsetY = table.Column<int>(type: "int", nullable: false),
                LevelOffsetX = table.Column<int>(type: "int", nullable: false),
                LevelOffsetY = table.Column<int>(type: "int", nullable: false),
                PfpOffsetX = table.Column<int>(type: "int", nullable: false),
                PfpOffsetY = table.Column<int>(type: "int", nullable: false),
                PfpRadiusFactor = table.Column<float>(type: "float", nullable: false),
                RankcardFlags = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_UserRankcardConfigs", x => x.Id))
            .Annotation("MySql:CharSet", "utf8mb4");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GuildLevelConfigs",
            schema: "Levels");

        migrationBuilder.DropTable(
            name: "GuildUserLevels",
            schema: "Levels");

        migrationBuilder.DropTable(
            name: "UserRankcardConfigs",
            schema: "Levels");
    }
}
