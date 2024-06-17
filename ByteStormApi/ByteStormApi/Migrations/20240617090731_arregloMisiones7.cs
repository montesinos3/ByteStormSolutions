using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteStormApi.Migrations
{
    /// <inheritdoc />
    public partial class arregloMisiones7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdMisiones",
                table: "Operativos");

            migrationBuilder.DropColumn(
                name: "IdEquipos",
                table: "Misiones");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdMisiones",
                table: "Operativos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdEquipos",
                table: "Misiones",
                type: "TEXT",
                nullable: true);
        }
    }
}
