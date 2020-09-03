using Microsoft.EntityFrameworkCore.Migrations;

namespace BlossomProduct.Core.Migrations
{
    public partial class AlterProductSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "ShortDescription",
                table: "Products",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Group", "LongDescription", "Name", "Price", "ShortDescription" },
                values: new object[] { 101, 2, "You will love it, plus it brighten your skin.", "Body Cream", 20.00m, "Nice to the skin." });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Group", "LongDescription", "Name", "Price", "ShortDescription" },
                values: new object[] { 102, 1, "You will love to eat it all day long.", "Chin-Chin", 5.00m, " Very nice and sweet." });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.AlterColumn<string>(
                name: "ShortDescription",
                table: "Products",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Group", "LongDescription", "Name", "Price", "ShortDescription" },
                values: new object[] { 1, 1, "You will love to eat it all day long", "Akara Chips", 200.00m, "Very nice and sweet" });
        }
    }
}
