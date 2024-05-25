using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebChoThueXe.Migrations
{
    public partial class addcolumnrating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Ratings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Ratings");
        }
    }
}
