using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MYPM.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SL = table.Column<string>(type: "text", nullable: false),
                    CustomerName = table.Column<string>(type: "text", nullable: false),
                    MobileNumber = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OrderFor = table.Column<string>(type: "text", nullable: false),
                    PaidAmount = table.Column<long>(type: "bigint", nullable: false),
                    DueAmount = table.Column<long>(type: "bigint", nullable: false),
                    TotalAmount = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArabianOrders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Length = table.Column<decimal>(type: "numeric", nullable: false),
                    Tira = table.Column<decimal>(type: "numeric", nullable: false),
                    Hata = table.Column<decimal>(type: "numeric", nullable: false),
                    Cuff = table.Column<decimal>(type: "numeric", nullable: false),
                    Mohori = table.Column<decimal>(type: "numeric", nullable: false),
                    Rakaba = table.Column<decimal>(type: "numeric", nullable: false),
                    Ness = table.Column<decimal>(type: "numeric", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArabianOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArabianOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PanjabiOrders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Length = table.Column<decimal>(type: "numeric", nullable: false),
                    Sina = table.Column<decimal>(type: "numeric", nullable: false),
                    Komor = table.Column<decimal>(type: "numeric", nullable: false),
                    Hata = table.Column<decimal>(type: "numeric", nullable: false),
                    Cuff = table.Column<decimal>(type: "numeric", nullable: false),
                    Mohori = table.Column<decimal>(type: "numeric", nullable: false),
                    Rakaba = table.Column<decimal>(type: "numeric", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PanjabiOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PanjabiOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelowerOrders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Length = table.Column<decimal>(type: "numeric", nullable: false),
                    Hip = table.Column<decimal>(type: "numeric", nullable: false),
                    Komor = table.Column<decimal>(type: "numeric", nullable: false),
                    Ness = table.Column<decimal>(type: "numeric", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelowerOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelowerOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArabianOrders_OrderId",
                table: "ArabianOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PanjabiOrders_OrderId",
                table: "PanjabiOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SelowerOrders_OrderId",
                table: "SelowerOrders",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArabianOrders");

            migrationBuilder.DropTable(
                name: "PanjabiOrders");

            migrationBuilder.DropTable(
                name: "SelowerOrders");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
