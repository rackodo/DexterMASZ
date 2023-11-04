using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Greeting.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "Greeting");

        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "GreeterConfigs",
            schema: "Greeting",
            columns: table => new
            {
                GuildId = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                AllowedGreetChannels = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                AllowedGreetRoles = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                DisallowedMuteRoles = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                DisallowedMuteExistence = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                PunishmentTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                LoggingChannel = table.Column<ulong>(type: "bigint unsigned", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_GreeterConfigs", x => x.GuildId))
            .Annotation("MySql:CharSet", "utf8mb4");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
            name: "GreeterConfigs",
            schema: "Greeting");
}
