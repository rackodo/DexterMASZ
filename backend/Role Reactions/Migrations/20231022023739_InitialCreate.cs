using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoleReactions.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "RoleReactions");

        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "RoleReactionsMenu",
            schema: "RoleReactions",
            columns: table => new
            {
                GuildId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                ChannelId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                MenuName = table.Column<string>(type: "varchar(255)", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                MessageId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                Roles = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Emotes = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table => table.PrimaryKey("PK_RoleReactionsMenu", x => new { x.GuildId, x.ChannelId, x.MenuName }))
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "UserRoles",
            schema: "RoleReactions",
            columns: table => new
            {
                GuildId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                UserId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                RoleIds = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table => table.PrimaryKey("PK_UserRoles", x => new { x.GuildId, x.UserId }))
            .Annotation("MySql:CharSet", "utf8mb4");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "RoleReactionsMenu",
            schema: "RoleReactions");

        migrationBuilder.DropTable(
            name: "UserRoles",
            schema: "RoleReactions");
    }
}
