using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestWebApplication.Migrations
{
    public partial class Connections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SaleEntries_ProductId",
                table: "SaleEntries",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleEntries_SaleId",
                table: "SaleEntries",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleEntries_Products_ProductId",
                table: "SaleEntries",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleEntries_Sales_SaleId",
                table: "SaleEntries",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "SaleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleEntries_Products_ProductId",
                table: "SaleEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleEntries_Sales_SaleId",
                table: "SaleEntries");

            migrationBuilder.DropIndex(
                name: "IX_SaleEntries_ProductId",
                table: "SaleEntries");

            migrationBuilder.DropIndex(
                name: "IX_SaleEntries_SaleId",
                table: "SaleEntries");
        }
    }
}
