using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trixx.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddFormDefaults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FormDeafaults",
                columns: table => new
                {
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Cartoon = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormDeafaults", x => x.Type);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEA8rDqbjw3etvsIkGrefxrpXHnaFGKGMEWKo0uQoupCWua1I+Shxf/tt+GDyTozeKQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormDeafaults");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGnfjdmhPrQkm9Ba7Qv1aAzVUXErVqpuIxIbzjprNsPD/KW4a3im8VwOOHSV7+WKWg==");
        }
    }
}
