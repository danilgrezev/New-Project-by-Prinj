using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend6.Data.Migrations
{
    public partial class AddModelsANDAddGradesANDAddPartsANDAddDetailsANDAddAttacgmentsANDAddBaskets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "Baskets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorId = table.Column<string>(nullable: false),
                    FullPrice = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baskets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Baskets_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
               name: "CarBrands",
               columns: table => new
               {
                   Id = table.Column<Guid>(nullable: false),                  
                   Description = table.Column<string>(nullable: true),
                   PathBrand = table.Column<string>(nullable: false),
                   Name = table.Column<string>(nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_CarBrands", x => x.Id);
                   
               });

            migrationBuilder.CreateTable(
                name: "CarModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CarBrandId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ModelPath = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarModels_CarBrands_CarBrandId",
                        column: x => x.CarBrandId,
                        principalTable: "CarBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarGrades",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CarModelId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    GradePath = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarGrades_CarModels_CarModelId",
                        column: x => x.CarModelId,
                        principalTable: "CarModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CarGradeId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    PartPath = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarParts_CarGrades_CarGradeId",
                        column: x => x.CarGradeId,
                        principalTable: "CarGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });           

            migrationBuilder.CreateTable(
                name: "CarDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BasketId = table.Column<Guid>(nullable: false),
                    CarPartId = table.Column<Guid>(nullable: false),
                    Cost = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DetailPath = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarDetails_Baskets_BasketId",
                        column: x => x.BasketId,
                        principalTable: "Baskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarDetails_CarParts_CarPartId",
                        column: x => x.CarPartId,
                        principalTable: "CarParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CarDetailId = table.Column<Guid>(nullable: false),
                    FilePath = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_CarDetails_CarDetailId",
                        column: x => x.CarDetailId,
                        principalTable: "CarDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<string>(
               name: "BrandPath",
               table: "CarBrands",
               nullable: false,
               defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_CarDetailId",
                table: "Attachments",
                column: "CarDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_CreatorId",
                table: "Baskets",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CarDetails_BasketId",
                table: "CarDetails",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_CarDetails_CarPartId",
                table: "CarDetails",
                column: "CarPartId");

            migrationBuilder.CreateIndex(
                name: "IX_CarGrades_CarModelId",
                table: "CarGrades",
                column: "CarModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CarModels_CarBrandId",
                table: "CarModels",
                column: "CarBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_CarParts_CarGradeId",
                table: "CarParts",
                column: "CarGradeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "CarDetails");

            migrationBuilder.DropTable(
                name: "Baskets");

            migrationBuilder.DropTable(
                name: "CarParts");

            migrationBuilder.DropTable(
                name: "CarGrades");

            migrationBuilder.DropTable(
                name: "CarModels");

            migrationBuilder.DropColumn(
                name: "BrandPath",
                table: "CarBrands");
        }
    }
}
