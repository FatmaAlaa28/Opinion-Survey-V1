using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Opinion_Survey.Migrations
{
    public partial class deleteSentimentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sentimentResults");

            migrationBuilder.AddColumn<DateTime>(
                name: "AnalyzedAt",
                table: "Answers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnalyzedAt",
                table: "Answers");

            migrationBuilder.CreateTable(
                name: "sentimentResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnalyzedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sentiment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sentimentResults", x => x.Id);
                });
        }
    }
}
