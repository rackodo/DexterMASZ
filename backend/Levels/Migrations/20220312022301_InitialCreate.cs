using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Levels.Migrations
{
    public partial class InitialCreate : Migration
    {
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
                    Coefficients = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    XPInterval = table.Column<int>(type: "int", nullable: false),
                    MinimumTextXPGiven = table.Column<int>(type: "int", nullable: false),
                    MaximumTextXPGiven = table.Column<int>(type: "int", nullable: false),
                    MinimumVoiceXPGiven = table.Column<int>(type: "int", nullable: false),
                    MaximumVoiceXPGiven = table.Column<int>(type: "int", nullable: false),
                    LevelUpTemplate = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VoiceLevelUpChannel = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    TextLevelUpChannel = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    DisabledXPChannels = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HandleRoles = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SendTextLevelUps = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SendVoiceLevelUps = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    VoiceXPCountMutedMembers = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    VoiceXPRequiredMembers = table.Column<int>(type: "int", nullable: false),
                    NicknameDisabledRole = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    NicknameDisabledReplacement = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    RankcardImageSizeLimit = table.Column<int>(type: "int", nullable: false),
                    RankcardImageRequiredLevel = table.Column<int>(type: "int", nullable: false),
                    Levels = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LevelUpMessageOverrides = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildLevelConfigs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GuildUserLevels",
                schema: "Levels",
                columns: table => new
                {
                    Token = table.Column<string>(type: "char(22)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    GuildId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Textxp = table.Column<long>(type: "bigint", nullable: false),
                    Voicexp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildUserLevels", x => x.Token);
                })
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
                    Background = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    rankcardFlags = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRankcardConfigs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

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
}
