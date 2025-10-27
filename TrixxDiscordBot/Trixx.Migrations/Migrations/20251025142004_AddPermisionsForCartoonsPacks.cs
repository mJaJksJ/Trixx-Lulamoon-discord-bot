using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trixx.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddPermisionsForCartoonsPacks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Permissions",
                value: "[0,1,2,3,4,5,6,7,8,9,10,11,12]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Permissions",
                value: "[0,1,2,3,4,5,6,7,7,8,8]");
        }
    }
}
