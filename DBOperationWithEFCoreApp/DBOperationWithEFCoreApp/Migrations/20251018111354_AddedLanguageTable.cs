using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBOperationWithEFCoreApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedLanguageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LanguageID",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_LanguageID",
                table: "Books",
                column: "LanguageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Languages_LanguageID",
                table: "Books",
                column: "LanguageID",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Languages_LanguageID",
                table: "Books");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Books_LanguageID",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "LanguageID",
                table: "Books");
        }
    }
}
