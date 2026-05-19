using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNPMFastFood.Migrations
{
    public partial class AddImageUrlOnly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "OrderDetails");
        }
    }
}