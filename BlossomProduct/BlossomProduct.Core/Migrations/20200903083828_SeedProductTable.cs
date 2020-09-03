using Microsoft.EntityFrameworkCore.Migrations;

namespace BlossomProduct.Core.Migrations
{
    public partial class SeedProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Group", "LongDescription", "Name", "Price", "ShortDescription" },
                values: new object[] { 1, 1, "You will love to eat it all day long", "Akara Chips", 200.00m, "Very nice and sweet" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
