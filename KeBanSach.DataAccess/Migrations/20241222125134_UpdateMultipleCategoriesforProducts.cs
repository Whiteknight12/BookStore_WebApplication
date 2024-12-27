using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeBanSach.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMultipleCategoriesforProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BangDanhMucCopy_BangDanhMuc_DanhMucId",
                table: "BangDanhMucCopy");

            migrationBuilder.DropIndex(
                name: "IX_BangDanhMucCopy_DanhMucId",
                table: "BangDanhMucCopy");

            migrationBuilder.DropColumn(
                name: "DanhMucId",
                table: "BangDanhMucCopy");

            migrationBuilder.AddColumn<string>(
                name: "fullname",
                table: "BangSellCanvas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "userid",
                table: "BangSellCanvas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DanhMucName",
                table: "BangDanhMucCopy",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fullname",
                table: "BangSellCanvas");

            migrationBuilder.DropColumn(
                name: "userid",
                table: "BangSellCanvas");

            migrationBuilder.DropColumn(
                name: "DanhMucName",
                table: "BangDanhMucCopy");

            migrationBuilder.AddColumn<int>(
                name: "DanhMucId",
                table: "BangDanhMucCopy",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BangDanhMucCopy_DanhMucId",
                table: "BangDanhMucCopy",
                column: "DanhMucId");

            migrationBuilder.AddForeignKey(
                name: "FK_BangDanhMucCopy_BangDanhMuc_DanhMucId",
                table: "BangDanhMucCopy",
                column: "DanhMucId",
                principalTable: "BangDanhMuc",
                principalColumn: "DanhMucId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
