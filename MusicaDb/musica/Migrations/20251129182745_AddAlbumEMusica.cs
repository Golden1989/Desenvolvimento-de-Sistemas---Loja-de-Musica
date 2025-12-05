using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace musica.Migrations
{
    /// <inheritdoc />
    public partial class AddAlbumEMusica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlbumId",
                table: "Musicas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Genero",
                table: "Musicas",
                type: "TEXT",
                maxLength: 60,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Albuns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Ano = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albuns", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Musicas_AlbumId",
                table: "Musicas",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Albuns_Nome",
                table: "Albuns",
                column: "Nome",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Musicas_Albuns_AlbumId",
                table: "Musicas",
                column: "AlbumId",
                principalTable: "Albuns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Musicas_Albuns_AlbumId",
                table: "Musicas");

            migrationBuilder.DropTable(
                name: "Albuns");

            migrationBuilder.DropIndex(
                name: "IX_Musicas_AlbumId",
                table: "Musicas");

            migrationBuilder.DropColumn(
                name: "AlbumId",
                table: "Musicas");

            migrationBuilder.DropColumn(
                name: "Genero",
                table: "Musicas");
        }
    }
}
