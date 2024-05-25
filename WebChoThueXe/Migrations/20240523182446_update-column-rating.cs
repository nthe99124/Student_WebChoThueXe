using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebChoThueXe.Migrations
{
	public partial class updatecolumnrating : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "CreatedBy",
				table: "Ratings");


			migrationBuilder.AddColumn<string>(
				name: "CreatedByName",
				table: "Ratings",
				type: "nvarchar(max)",
				nullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "CreatedByName",
				table: "Ratings");


			migrationBuilder.AddColumn<int>(
				name: "CreatedBy",
				table: "Ratings",
				type: "int",
				nullable: true);
		}
	}
}
