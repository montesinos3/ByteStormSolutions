using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteStormApi.Migrations
{
    /// <inheritdoc />
    public partial class prueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipos_Misiones_MisionId",
                table: "Equipos");

            migrationBuilder.DropForeignKey(
                name: "FK_Misiones_Operativos_OperativoId",
                table: "Misiones");

            migrationBuilder.DropIndex(
                name: "IX_Misiones_OperativoId",
                table: "Misiones");

            migrationBuilder.DropIndex(
                name: "IX_Equipos_MisionId",
                table: "Equipos");

            migrationBuilder.DropColumn(
                name: "OperativoId",
                table: "Misiones");

            migrationBuilder.DropColumn(
                name: "MisionId",
                table: "Equipos");

            migrationBuilder.CreateIndex(
                name: "IX_Misiones_IdOperativo",
                table: "Misiones",
                column: "IdOperativo");

            migrationBuilder.CreateIndex(
                name: "IX_Equipos_IdMision",
                table: "Equipos",
                column: "IdMision");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipos_Misiones_IdMision",
                table: "Equipos",
                column: "IdMision",
                principalTable: "Misiones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Misiones_Operativos_IdOperativo",
                table: "Misiones",
                column: "IdOperativo",
                principalTable: "Operativos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipos_Misiones_IdMision",
                table: "Equipos");

            migrationBuilder.DropForeignKey(
                name: "FK_Misiones_Operativos_IdOperativo",
                table: "Misiones");

            migrationBuilder.DropIndex(
                name: "IX_Misiones_IdOperativo",
                table: "Misiones");

            migrationBuilder.DropIndex(
                name: "IX_Equipos_IdMision",
                table: "Equipos");

            migrationBuilder.AddColumn<long>(
                name: "OperativoId",
                table: "Misiones",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MisionId",
                table: "Equipos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Misiones_OperativoId",
                table: "Misiones",
                column: "OperativoId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipos_MisionId",
                table: "Equipos",
                column: "MisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipos_Misiones_MisionId",
                table: "Equipos",
                column: "MisionId",
                principalTable: "Misiones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Misiones_Operativos_OperativoId",
                table: "Misiones",
                column: "OperativoId",
                principalTable: "Operativos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
