using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteStormApi.Migrations
{
    /// <inheritdoc />
    public partial class tryNoLoops2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipos_Misiones_IdMision",
                table: "Equipos");

            migrationBuilder.DropForeignKey(
                name: "FK_Misiones_Operativos_IdOperativo",
                table: "Misiones");

            migrationBuilder.AlterColumn<long>(
                name: "IdOperativo",
                table: "Misiones",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<long>(
                name: "IdMision",
                table: "Equipos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipos_Misiones_IdMision",
                table: "Equipos",
                column: "IdMision",
                principalTable: "Misiones",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Misiones_Operativos_IdOperativo",
                table: "Misiones",
                column: "IdOperativo",
                principalTable: "Operativos",
                principalColumn: "Id");
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

            migrationBuilder.AlterColumn<long>(
                name: "IdOperativo",
                table: "Misiones",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "IdMision",
                table: "Equipos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

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
    }
}
