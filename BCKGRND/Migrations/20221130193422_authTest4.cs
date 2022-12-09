using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BCKGRND.Migrations
{
    public partial class authTest4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserPass",
                table: "Users",
                type: "nvarChar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarChar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarChar(30)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarChar(30)");

            migrationBuilder.AlterColumn<string>(
                name: "UserMail",
                table: "Users",
                type: "nvarChar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarChar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "Users",
                type: "nvarChar(30)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarChar(30)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserPass",
                keyValue: null,
                column: "UserPass",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserPass",
                table: "Users",
                type: "nvarChar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarChar(100)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserName",
                keyValue: null,
                column: "UserName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarChar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarChar(30)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserMail",
                keyValue: null,
                column: "UserMail",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserMail",
                table: "Users",
                type: "nvarChar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarChar(100)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Salt",
                keyValue: null,
                column: "Salt",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "Users",
                type: "nvarChar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarChar(30)",
                oldNullable: true);
        }
    }
}
