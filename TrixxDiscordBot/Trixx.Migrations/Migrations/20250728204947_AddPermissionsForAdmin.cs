using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trixx.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionsForAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Permissions",
                value: "[0,1,2,3,4,5,6]");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGnfjdmhPrQkm9Ba7Qv1aAzVUXErVqpuIxIbzjprNsPD/KW4a3im8VwOOHSV7+WKWg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Permissions",
                value: "[0]");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFdE7S8emyJvBdxfdadTlYiaAp6helKCP25YizHnoT02qghyn+S0ZRvFzkdM+JEVgA==");
        }
    }
}
