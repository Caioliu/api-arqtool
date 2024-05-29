using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace caiobadev_api_arqtool.Migrations
{
    public partial class CamposAdicionais : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantidadeAtividades",
                table: "Projetos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantidadeEtapas",
                table: "Projetos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "QuantidadeHoras",
                table: "Projetos",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ValorTotalDasEtapas",
                table: "Projetos",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Complexidade",
                table: "Etapas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "QuantidadeHoras",
                table: "Etapas",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ValorDaEtapa",
                table: "Etapas",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantidadeAtividades",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "QuantidadeEtapas",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "QuantidadeHoras",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "ValorTotalDasEtapas",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "Complexidade",
                table: "Etapas");

            migrationBuilder.DropColumn(
                name: "QuantidadeHoras",
                table: "Etapas");

            migrationBuilder.DropColumn(
                name: "ValorDaEtapa",
                table: "Etapas");
        }
    }
}
