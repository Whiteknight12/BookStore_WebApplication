using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeBanSach.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUsernametoSellCanvas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "BangSellCanvas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "username",
                table: "BangSellCanvas");
        }
    }
}
