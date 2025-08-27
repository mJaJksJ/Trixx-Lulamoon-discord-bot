using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trixx.Cartoons.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddNormalizedAllNamesToDictionaryCartoon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedAllNames",
                table: "DictionaryCartoons",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDateTime",
                table: "CartoonSystemObject",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedAllNames",
                table: "DictionaryCartoons");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDateTime",
                table: "CartoonSystemObject",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
