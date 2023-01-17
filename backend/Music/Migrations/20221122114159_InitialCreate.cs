using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Music.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Music");

            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                    name: "StartTimes",
                    schema: "Music",
                    columns: table => new
                    {
                        Id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                        RadioStartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_StartTimes", x => x.Id);
                    })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
                name: "StartTimes",
                schema: "Music");
    }
}
