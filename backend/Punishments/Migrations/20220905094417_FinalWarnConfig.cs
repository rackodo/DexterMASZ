using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Punishments.Migrations;

public partial class FinalWarnConfig : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "PunishmentConfig",
			schema: "Punishments",
			columns: table => new
			{
				GuildId = table.Column<ulong>(type: "bigint unsigned", nullable: false)
					.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
				FinalWarnMuteTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
				PointMuteTimes = table.Column<string>(type: "longtext", nullable: true)
					.Annotation("MySql:CharSet", "utf8mb4")
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_PunishmentConfig", x => x.GuildId);
			})
			.Annotation("MySql:CharSet", "utf8mb4");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "PunishmentConfig",
			schema: "Punishments");
	}
}
