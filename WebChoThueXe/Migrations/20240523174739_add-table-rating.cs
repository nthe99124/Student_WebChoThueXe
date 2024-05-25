using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebChoThueXe.Migrations
{
	public partial class addtablerating : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Ratings",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ProductId = table.Column<int>(type: "int", nullable: true),
					CreatedBy = table.Column<int>(type: "int", nullable: true),
					CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Star = table.Column<int>(type: "int", nullable: false),
					LinkImage1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
					LinkImage2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
					LinkImage3 = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Ratings", x => x.Id);
					table.ForeignKey(
						name: "FK_Ratings_Products_ProductId",
						column: x => x.ProductId,
						principalTable: "Products",
						principalColumn: "Id");
				});

			migrationBuilder.CreateIndex(
				name: "IX_Ratings_ProductId",
				table: "Ratings",
				column: "ProductId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Ratings");
		}
	}
}
