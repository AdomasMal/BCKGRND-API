using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BCKGRND.Migrations
{
    public partial class tagTest4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationTag_Tag_TagsID",
                table: "LocationTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationTag_Tags_TagsID",
                table: "LocationTag",
                column: "TagsID",
                principalTable: "Tags",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationTag_Tags_TagsID",
                table: "LocationTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tag");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationTag_Tag_TagsID",
                table: "LocationTag",
                column: "TagsID",
                principalTable: "Tag",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
