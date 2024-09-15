using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Opinion_Survey.Migrations
{
    public partial class imageQuestion2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagepath",
                table: "Forms");

            migrationBuilder.AddColumn<string>(
                name: "Imagepath",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagepath",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "Imagepath",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
