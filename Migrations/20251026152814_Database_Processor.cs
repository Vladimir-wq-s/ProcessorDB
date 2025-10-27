using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProcessorDB.Migrations
{
    /// <inheritdoc />
    public partial class Database_Processor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Processors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    ManufacturerId = table.Column<int>(type: "INTEGER", nullable: false),
                    CountryId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReleaseYear = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Processors_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processors_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProcessorId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WarrantyPeriod = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Points = table.Column<int>(type: "INTEGER", nullable: false),
                    Promotion = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionInfos_Processors_ProcessorId",
                        column: x => x.ProcessorId,
                        principalTable: "Processors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechSpecs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProcessorId = table.Column<int>(type: "INTEGER", nullable: false),
                    TechProcess = table.Column<string>(type: "TEXT", nullable: false),
                    Frequency = table.Column<double>(type: "REAL", nullable: false),
                    CacheL3 = table.Column<string>(type: "TEXT", nullable: false),
                    Cores = table.Column<int>(type: "INTEGER", nullable: false),
                    Slot = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechSpecs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechSpecs_Processors_ProcessorId",
                        column: x => x.ProcessorId,
                        principalTable: "Processors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "USA" },
                    { 2, "Taiwan" }
                });

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "AMD" },
                    { 2, "Intel" }
                });

            migrationBuilder.InsertData(
                table: "Processors",
                columns: new[] { "Id", "CountryId", "ManufacturerId", "Model", "Name", "ReleaseYear" },
                values: new object[,]
                {
                    { 1, 2, 1, "Ryzen 5 8400F", "Ryzen 5 8400F", 2024 },
                    { 2, 2, 1, "Ryzen 7 7800X3D", "Ryzen 7 7800X3D", 2023 },
                    { 3, 1, 2, "Core i5-12400F", "Core i5-12400F", 2022 },
                    { 4, 1, 2, "Core i7-13700K", "Core i7-13700K", 2023 }
                });

            migrationBuilder.InsertData(
                table: "ProductionInfos",
                columns: new[] { "Id", "Points", "Price", "ProcessorId", "ProductionDate", "Promotion", "WarrantyPeriod" },
                values: new object[,]
                {
                    { 1, 1200, 199m, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 36 },
                    { 2, 1800, 449m, 2, new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 36 },
                    { 3, 1100, 179m, 3, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 36 },
                    { 4, 1600, 409m, 4, new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 36 }
                });

            migrationBuilder.InsertData(
                table: "TechSpecs",
                columns: new[] { "Id", "CacheL3", "Cores", "Frequency", "ProcessorId", "Slot", "TechProcess" },
                values: new object[,]
                {
                    { 1, "16MB", 6, 4.7000000000000002, 1, "AM5", "4nm" },
                    { 2, "96MB", 8, 4.2000000000000002, 2, "AM5", "5nm" },
                    { 3, "18MB", 6, 2.5, 3, "LGA 1700", "10nm" },
                    { 4, "30MB", 16, 3.3999999999999999, 4, "LGA 1700", "10nm" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Processors_CountryId",
                table: "Processors",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Processors_ManufacturerId",
                table: "Processors",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionInfos_ProcessorId",
                table: "ProductionInfos",
                column: "ProcessorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechSpecs_ProcessorId",
                table: "TechSpecs",
                column: "ProcessorId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionInfos");

            migrationBuilder.DropTable(
                name: "TechSpecs");

            migrationBuilder.DropTable(
                name: "Processors");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Manufacturers");
        }
    }
}
