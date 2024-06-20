using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace caiobadev_api_arqtool.Migrations
{
    public partial class calcularProjeto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AcrescimoTotalComplexidade",
                table: "Projetos",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AcrescimoTotalUrgencia",
                table: "Projetos",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Complexidade",
                table: "Projetos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFinal",
                table: "Projetos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInicial",
                table: "Projetos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantidadeAmbientes",
                table: "Projetos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantidadeAmbientesMolhados",
                table: "Projetos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Urgencia",
                table: "Projetos",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcrescimoTotalComplexidade",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "AcrescimoTotalUrgencia",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "Complexidade",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "DataFinal",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "DataInicial",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "QuantidadeAmbientes",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "QuantidadeAmbientesMolhados",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "Urgencia",
                table: "Projetos");
        }
    }
}
