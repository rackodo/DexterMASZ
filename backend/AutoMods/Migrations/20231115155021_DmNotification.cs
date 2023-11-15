using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoMods.Migrations;

/// <inheritdoc />
public partial class DmNotification : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Discriminator",
            schema: "AutoMods",
            table: "AutoModEvents");

        migrationBuilder.AddColumn<bool>(
            name: "SendDmNotification",
            schema: "AutoMods",
            table: "AutoModConfigs",
            type: "tinyint(1)",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "SendDmNotification",
            schema: "AutoMods",
            table: "AutoModConfigs");

        migrationBuilder.AddColumn<string>(
            name: "Discriminator",
            schema: "AutoMods",
            table: "AutoModEvents",
            type: "longtext",
            nullable: true)
            .Annotation("MySql:CharSet", "utf8mb4");
    }
}
