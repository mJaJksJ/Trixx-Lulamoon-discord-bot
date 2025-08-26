using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Trixx.Cartoons.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddCartoonSystemObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SystemObjectId",
                table: "DictionaryStudios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SystemObjectId",
                table: "DictionaryCartoons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CartoonSystemObject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    DictionaryCartoonId = table.Column<int>(type: "integer", nullable: true),
                    DictionaryStudioId = table.Column<int>(type: "integer", nullable: true),
                    Creator = table.Column<string>(type: "text", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartoonSystemObject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartoonSystemObject_DictionaryCartoons_DictionaryCartoonId",
                        column: x => x.DictionaryCartoonId,
                        principalTable: "DictionaryCartoons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartoonSystemObject_DictionaryStudios_DictionaryStudioId",
                        column: x => x.DictionaryStudioId,
                        principalTable: "DictionaryStudios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartoonSystemObject_DictionaryCartoonId",
                table: "CartoonSystemObject",
                column: "DictionaryCartoonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartoonSystemObject_DictionaryStudioId",
                table: "CartoonSystemObject",
                column: "DictionaryStudioId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartoonSystemObject");

            migrationBuilder.DropColumn(
                name: "SystemObjectId",
                table: "DictionaryStudios");

            migrationBuilder.DropColumn(
                name: "SystemObjectId",
                table: "DictionaryCartoons");
        }
    }
}
