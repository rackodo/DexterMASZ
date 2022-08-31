using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Levels.Migrations
{
    public partial class ModifiedOffsetsToIntegers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TitleOffsetY",
                schema: "Levels",
                table: "UserRankcardConfigs",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "TitleOffsetX",
                schema: "Levels",
                table: "UserRankcardConfigs",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "PfpOffsetY",
                schema: "Levels",
                table: "UserRankcardConfigs",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "PfpOffsetX",
                schema: "Levels",
                table: "UserRankcardConfigs",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "LevelOffsetY",
                schema: "Levels",
                table: "UserRankcardConfigs",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "LevelOffsetX",
                schema: "Levels",
                table: "UserRankcardConfigs",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "TitleOffsetY",
                schema: "Levels",
                table: "UserRankcardConfigs",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "TitleOffsetX",
                schema: "Levels",
                table: "UserRankcardConfigs",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "PfpOffsetY",
                schema: "Levels",
                table: "UserRankcardConfigs",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "PfpOffsetX",
                schema: "Levels",
                table: "UserRankcardConfigs",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "LevelOffsetY",
                schema: "Levels",
                table: "UserRankcardConfigs",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "LevelOffsetX",
                schema: "Levels",
                table: "UserRankcardConfigs",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
