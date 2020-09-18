using Microsoft.EntityFrameworkCore.Migrations;

namespace BlossomProduct.Core.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    Message = table.Column<string>(maxLength: 6000, nullable: false),
                    ContactMe = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 500, nullable: false),
                    Group = table.Column<int>(nullable: false),
                    ShortDescription = table.Column<string>(maxLength: 2000, nullable: false),
                    LongDescription = table.Column<string>(maxLength: 5000, nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    PhotoPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Group", "LongDescription", "Name", "PhotoPath", "Price", "ShortDescription" },
                values: new object[] { 101, 2, "You will love it, plus it brighten your skin.", "Body Cream", null, 20.00m, "Nice to the skin." });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Group", "LongDescription", "Name", "PhotoPath", "Price", "ShortDescription" },
                values: new object[] { 102, 1, "You will love to eat it all day long.", "Chin-Chin", null, 5.00m, " Very nice and sweet." });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
