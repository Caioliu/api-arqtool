using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace caiobadev_api_arqtool.Migrations
{
    public partial class Elavaioutra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotalAmbientesMolhados",
                table: "Projetos",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotalProjeto",
                table: "Projetos",
                type: "decimal(65,30)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorTotalAmbientesMolhados",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "ValorTotalProjeto",
                table: "Projetos");
        }
    }
}
