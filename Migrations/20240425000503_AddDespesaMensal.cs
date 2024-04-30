using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace caiobadev_api_arqtool.Migrations
{
    public partial class AddDespesaMensal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DespesasMensais",
                columns: table => new
                {
                    DespesaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GastoMensal = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PorcentagemGastoTotal = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    GastoAnual = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Hora = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    UsuarioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DespesasMensais", x => x.DespesaId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DespesasMensais");
        }
    }
}
