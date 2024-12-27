using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeBanSach.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForMultipleCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "sanphamname",
                table: "BangSellCanvas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BangSanPhamVaDanhMuc",
                columns: table => new
                {
                    SanPhamVaDanhMucId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    spid = table.Column<int>(type: "int", nullable: true),
                    dmid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BangSanPhamVaDanhMuc", x => x.SanPhamVaDanhMucId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BangSanPhamVaDanhMuc");

            migrationBuilder.DropColumn(
                name: "sanphamname",
                table: "BangSellCanvas");
        }
    }
}
