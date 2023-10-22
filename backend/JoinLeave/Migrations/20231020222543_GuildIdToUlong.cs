using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoinLeave.Migrations;

/// <inheritdoc />
public partial class GuildIdToUlong : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AlterColumn<ulong>(
            name: "GuildId",
            schema: "JoinLeave",
            table: "JoinLeaveConfig",
            type: "bigint unsigned",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int")
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
            .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.AlterColumn<int>(
            name: "GuildId",
            schema: "JoinLeave",
            table: "JoinLeaveConfig",
            type: "int",
            nullable: false,
            oldClrType: typeof(ulong),
            oldType: "bigint unsigned")
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
            .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
}
