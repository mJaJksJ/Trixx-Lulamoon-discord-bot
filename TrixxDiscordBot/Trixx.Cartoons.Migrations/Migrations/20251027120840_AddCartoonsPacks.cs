using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Trixx.Cartoons.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddCartoonsPacks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartoonsPacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SystemObjectId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartoonsPacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartoonsPacks_CartoonSystemObject_SystemObjectId",
                        column: x => x.SystemObjectId,
                        principalTable: "CartoonSystemObject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartoonsPackLabelTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    CartoonsPackId = table.Column<int>(type: "integer", nullable: false),
                    SystemObjectId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartoonsPackLabelTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartoonsPackLabelTypes_CartoonSystemObject_SystemObjectId",
                        column: x => x.SystemObjectId,
                        principalTable: "CartoonSystemObject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartoonsPackLabelTypes_CartoonsPacks_CartoonsPackId",
                        column: x => x.CartoonsPackId,
                        principalTable: "CartoonsPacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackCartoons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DictionaryCartoonId = table.Column<int>(type: "integer", nullable: false),
                    CartoonsPackLabelTypeId = table.Column<int>(type: "integer", nullable: false),
                    SystemObjectId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackCartoons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackCartoons_CartoonSystemObject_SystemObjectId",
                        column: x => x.SystemObjectId,
                        principalTable: "CartoonSystemObject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackCartoons_CartoonsPackLabelTypes_CartoonsPackLabelTypeId",
                        column: x => x.CartoonsPackLabelTypeId,
                        principalTable: "CartoonsPackLabelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackCartoons_DictionaryCartoons_DictionaryCartoonId",
                        column: x => x.DictionaryCartoonId,
                        principalTable: "DictionaryCartoons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartoonsPackLabelTypes_CartoonsPackId",
                table: "CartoonsPackLabelTypes",
                column: "CartoonsPackId");

            migrationBuilder.CreateIndex(
                name: "IX_CartoonsPackLabelTypes_SystemObjectId",
                table: "CartoonsPackLabelTypes",
                column: "SystemObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CartoonsPacks_SystemObjectId",
                table: "CartoonsPacks",
                column: "SystemObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PackCartoons_CartoonsPackLabelTypeId",
                table: "PackCartoons",
                column: "CartoonsPackLabelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PackCartoons_DictionaryCartoonId",
                table: "PackCartoons",
                column: "DictionaryCartoonId");

            migrationBuilder.CreateIndex(
                name: "IX_PackCartoons_SystemObjectId",
                table: "PackCartoons",
                column: "SystemObjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackCartoons");

            migrationBuilder.DropTable(
                name: "CartoonsPackLabelTypes");

            migrationBuilder.DropTable(
                name: "CartoonsPacks");
        }
    }
}
