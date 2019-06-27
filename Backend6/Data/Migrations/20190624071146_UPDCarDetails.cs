using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend6.Data.Migrations
{
    public partial class UPDCarDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarDetails_Baskets_BasketId",
                table: "CarDetails");

            migrationBuilder.DropIndex(
                name: "IX_CarDetails_BasketId",
                table: "CarDetails");

            migrationBuilder.RenameColumn(
                name: "BrandPath",
                table: "CarBrands",
                newName: "PathBrand");

            migrationBuilder.AlterColumn<string>(
                name: "BasketId",
                table: "CarDetails",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "BasketId1",
                table: "CarDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarDetails_BasketId1",
                table: "CarDetails",
                column: "BasketId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CarDetails_Baskets_BasketId1",
                table: "CarDetails",
                column: "BasketId1",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarDetails_Baskets_BasketId1",
                table: "CarDetails");

            migrationBuilder.DropIndex(
                name: "IX_CarDetails_BasketId1",
                table: "CarDetails");

            migrationBuilder.DropColumn(
                name: "BasketId1",
                table: "CarDetails");

            migrationBuilder.RenameColumn(
                name: "PathBrand",
                table: "CarBrands",
                newName: "BrandPath");

            migrationBuilder.AlterColumn<Guid>(
                name: "BasketId",
                table: "CarDetails",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarDetails_BasketId",
                table: "CarDetails",
                column: "BasketId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarDetails_Baskets_BasketId",
                table: "CarDetails",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
