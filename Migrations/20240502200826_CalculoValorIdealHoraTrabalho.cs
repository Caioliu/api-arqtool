using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace caiobadev_api_arqtool.Migrations
{
    public partial class CalculoValorIdealHoraTrabalho : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ValoresIdeaisHoraTrabalho",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FaturamentoMensalDesejado = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ReservaFinanceira = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TotalDespesasMensais = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    HorasTrabalhadasPorDia = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DiasTrabalhadosPorSemana = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DiasTrabalhadosPorMes = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    HorasTrabalhadasPorMes = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    DiasFeriasPorAno = table.Column<int>(type: "int", nullable: false),
                    FaturamentoMensalMinimo = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    ValorMinimoHoraDeTrabalho = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    ValorIdealHoraDeTrabalho = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CustoFerias = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    PercentualCustoFerias = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    PercentualReservaFerias = table.Column<decimal>(type: "decimal(65,30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValoresIdeaisHoraTrabalho", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValoresIdeaisHoraTrabalho");
        }
    }
}
