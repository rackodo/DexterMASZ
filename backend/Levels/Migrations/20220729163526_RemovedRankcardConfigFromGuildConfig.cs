using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Levels.Migrations
{
    public partial class RemovedRankcardConfigFromGuildConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RankcardImageRequiredLevel",
                schema: "Levels",
                table: "GuildLevelConfigs");

            migrationBuilder.DropColumn(
                name: "RankcardImageSizeLimit",
                schema: "Levels",
                table: "GuildLevelConfigs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RankcardImageRequiredLevel",
                schema: "Levels",
                table: "GuildLevelConfigs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RankcardImageSizeLimit",
                schema: "Levels",
                table: "GuildLevelConfigs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
