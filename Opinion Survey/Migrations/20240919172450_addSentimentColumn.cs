using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Opinion_Survey.Migrations
{
    public partial class addSentimentColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SentimentResult",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentimentResult",
                table: "Answers");
        }
    }
}
