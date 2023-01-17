using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bot.Migrations;

public partial class UserLeftDatabase : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.CreateTable(
            name: "LeftUsers",
            schema: "Bot",
            columns: table => new
            {
                Id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                IsBot = table.Column<bool>(type: "tinyint(1)", nullable: false),
                Username = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                DiscriminatorValue = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                AvatarId = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                PublicFlags = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LeftUsers", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
            name: "LeftUsers",
            schema: "Bot");
}
