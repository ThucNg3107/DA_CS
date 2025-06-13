using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyThuVien.Migrations
{
    /// <inheritdoc />
    public partial class _17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LibraryCards",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryCards_UserId",
                table: "LibraryCards",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryCards_AspNetUsers_UserId",
                table: "LibraryCards",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryCards_AspNetUsers_UserId",
                table: "LibraryCards");

            migrationBuilder.DropIndex(
                name: "IX_LibraryCards_UserId",
                table: "LibraryCards");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LibraryCards");
        }
    }
}
