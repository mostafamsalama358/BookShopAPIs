using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Domains.Migrations
{
    /// <inheritdoc />
    public partial class CreateTbAuthorBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbBooks_TbAuthors_AuthorId",
                table: "TbBooks");

            migrationBuilder.DropIndex(
                name: "IX_TbBooks_AuthorId",
                table: "TbBooks");

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

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "TbBooks");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "TbBooks",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "TbBooks",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Biography",
                table: "TbAuthors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.CreateTable(
                name: "TbAuthorBooks",
                columns: table => new
                {
                    TbAuthorId = table.Column<int>(type: "int", nullable: false),
                    TbBookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbAuthorBooks", x => new { x.TbAuthorId, x.TbBookId });
                    table.ForeignKey(
                        name: "FK_TbAuthorBooks_TbAuthors_TbAuthorId",
                        column: x => x.TbAuthorId,
                        principalTable: "TbAuthors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbAuthorBooks_TbBooks_TbBookId",
                        column: x => x.TbBookId,
                        principalTable: "TbBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorBooks_TbBookId",
                table: "TbAuthorBooks",
                column: "TbBookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbAuthorBooks");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "TbBooks",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "TbBooks",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "TbBooks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Biography",
                table: "TbAuthors",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_TbBooks_AuthorId",
                table: "TbBooks",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbBooks_TbAuthors_AuthorId",
                table: "TbBooks",
                column: "AuthorId",
                principalTable: "TbAuthors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
