using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Domains.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TbAuthors",
                columns: new[] { "Id", "Biography", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 25, "", "Author One", " One" },
                    { 26, "", "Author ", "Author Two" }
                });

            migrationBuilder.InsertData(
                table: "TbBooks",
                columns: new[] { "Id", "AuthorId", "CategoryId", "ImagePath", "ImageUrl", "Title", "YearPublished" },
                values: new object[,]
                {
                    { 30, 1, 1, "", "", "Book One", 2020 },
                    { 32, 2, 2, "", "", "Book Two", 2021 }
                });

            migrationBuilder.InsertData(
                table: "TbCategories",
                columns: new[] { "Id", "CategoryName" },
                values: new object[,]
                {
                    { 30, "Fiction" },
                    { 31, "Non-Fiction" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TbAuthors",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "TbAuthors",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "TbBooks",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "TbBooks",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "TbCategories",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "TbCategories",
                keyColumn: "Id",
                keyValue: 31);
        }
    }
}
