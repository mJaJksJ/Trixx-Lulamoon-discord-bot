using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trixx.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFdE7S8emyJvBdxfdadTlYiaAp6helKCP25YizHnoT02qghyn+S0ZRvFzkdM+JEVgA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "trixx@trixx.trixx");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "NormalizedEmail",
                value: "TRIXX@TRIXX.TRIXX");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           throw new System.NotImplementedException();
        }
    }
}
