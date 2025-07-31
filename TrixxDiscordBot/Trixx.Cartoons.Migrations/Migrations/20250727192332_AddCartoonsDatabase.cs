using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Trixx.Cartoons.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddCartoonsDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DictionaryCartoons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AlternativeNames = table.Column<string>(type: "jsonb", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Sources = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryCartoons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryStudios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryStudios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryCatroonStudios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CartoonId = table.Column<int>(type: "integer", nullable: false),
                    DictionaryStudioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryCatroonStudios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictionaryCatroonStudios_DictionaryCartoons_CartoonId",
                        column: x => x.CartoonId,
                        principalTable: "DictionaryCartoons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DictionaryCatroonStudios_DictionaryStudios_DictionaryStudio~",
                        column: x => x.DictionaryStudioId,
                        principalTable: "DictionaryStudios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryCatroonStudios_CartoonId_DictionaryStudioId",
                table: "DictionaryCatroonStudios",
                columns: new[] { "CartoonId", "DictionaryStudioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryCatroonStudios_DictionaryStudioId",
                table: "DictionaryCatroonStudios",
                column: "DictionaryStudioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DictionaryCatroonStudios");

            migrationBuilder.DropTable(
                name: "DictionaryCartoons");

            migrationBuilder.DropTable(
                name: "DictionaryStudios");
        }
    }
}
