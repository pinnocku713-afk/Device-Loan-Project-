using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Device_Loan_Project.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetTag = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PurchaseYear = table.Column<int>(type: "int", nullable: false),
                    IsOnLoan = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "AssetTag", "Category", "IsOnLoan", "Name", "PurchaseYear" },
                values: new object[,]
                {
                    { 1, "ECC-0042", "Laptop", "yes", "Dell Latitude", 2023 },
                    { 2, "ECC-0007", "Laptop", "no", "HP ProBook 450", 2019 },
                    { 3, "ECC-0115", "Tablet", "yes", "iPad 9th generation", 2022 },
                    { 4, "ECC-0090", "Tablet", "no", "Samsung Galexy Tab A8", 2021 },
                    { 5, "ECC-0023", "Laptop", "no", "Lenovo ThinkPad E 14", 2017 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_AssetTag",
                table: "Devices",
                column: "AssetTag",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}
