using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Levels.Migrations
{
    public partial class XPIntoXpCaseShift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "XPInterval",
                schema: "Levels",
                table: "GuildLevelConfigs",
                newName: "XpInterval");

            migrationBuilder.RenameColumn(
                name: "VoiceXPRequiredMembers",
                schema: "Levels",
                table: "GuildLevelConfigs",
                newName: "VoiceXpRequiredMembers");

            migrationBuilder.RenameColumn(
                name: "VoiceXPCountMutedMembers",
                schema: "Levels",
                table: "GuildLevelConfigs",
                newName: "VoiceXpCountMutedMembers");

            migrationBuilder.RenameColumn(
                name: "MinimumVoiceXPGiven",
                schema: "Levels",
                table: "GuildLevelConfigs",
                newName: "MinimumVoiceXpGiven");

            migrationBuilder.RenameColumn(
                name: "MinimumTextXPGiven",
                schema: "Levels",
                table: "GuildLevelConfigs",
                newName: "MinimumTextXpGiven");

            migrationBuilder.RenameColumn(
                name: "MaximumVoiceXPGiven",
                schema: "Levels",
                table: "GuildLevelConfigs",
                newName: "MaximumVoiceXpGiven");

            migrationBuilder.RenameColumn(
                name: "MaximumTextXPGiven",
                schema: "Levels",
                table: "GuildLevelConfigs",
                newName: "MaximumTextXpGiven");

            migrationBuilder.RenameColumn(
                name: "DisabledXPChannels",
                schema: "Levels",
                table: "GuildLevelConfigs",
                newName: "DisabledXpChannels");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "XpInterval",
                schema: "Levels",
                table: "GuildLevelConfigs",
                newName: "XPInterval");

            migrationBuilder.RenameColumn(
                name: "VoiceXpRequiredMembers",
                schema: "Levels",
                table: "GuildLevelConfigs",
                newName: "VoiceXPRequiredMembers");

            migrationBuilder.RenameColumn(
                name: "VoiceXpCountMutedMembers",
                schema: "Levels",
                table: "GuildLevelConfigs",
                newName: "VoiceXPCountMutedMembers");

            migrationBuilder.RenameColumn(
                name: "MinimumVoiceXpGiven",
                schema: "Levels",
                table: "GuildLevelConfigs",
                newName: "MinimumVoiceXPGiven");

            migrationBuilder.RenameColumn(
                name: "MinimumTextXpGiven",
                schema: "Levels",
                table: "GuildLevelConfigs",
                newName: "MinimumTextXPGiven");

            migrationBuilder.RenameColumn(
                name: "MaximumVoiceXpGiven",
                schema: "Levels",
                table: "GuildLevelConfigs",
                newName: "MaximumVoiceXPGiven");

            migrationBuilder.RenameColumn(
                name: "MaximumTextXpGiven",
                schema: "Levels",
                table: "GuildLevelConfigs",
                newName: "MaximumTextXPGiven");

            migrationBuilder.RenameColumn(
                name: "DisabledXpChannels",
                schema: "Levels",
                table: "GuildLevelConfigs",
                newName: "DisabledXPChannels");
        }
    }
}
