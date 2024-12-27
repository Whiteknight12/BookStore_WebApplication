using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeBanSach.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCopyofDanhMuctoDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BangDanhMucCopy",
                columns: table => new
                {
                    DanhMucCopyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dmid = table.Column<int>(type: "int", nullable: false),
                    DanhMucId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BangDanhMucCopy", x => x.DanhMucCopyId);
                    table.ForeignKey(
                        name: "FK_BangDanhMucCopy_BangDanhMuc_DanhMucId",
                        column: x => x.DanhMucId,
                        principalTable: "BangDanhMuc",
                        principalColumn: "DanhMucId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BangDanhMucCopy_DanhMucId",
                table: "BangDanhMucCopy",
                column: "DanhMucId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BangDanhMucCopy");
        }
    }
}
