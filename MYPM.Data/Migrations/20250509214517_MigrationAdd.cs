using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYPM.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigrationAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Ber",
                table: "ArabianOrders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Komor",
                table: "ArabianOrders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ber",
                table: "ArabianOrders");

            migrationBuilder.DropColumn(
                name: "Komor",
                table: "ArabianOrders");
        }
    }
}
