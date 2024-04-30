using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace caiobadev_api_arqtool.Migrations
{
    public partial class AddValorTotalDespesaMensal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotal",
                table: "DespesasMensais",
                type: "decimal(65,30)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorTotal",
                table: "DespesasMensais");
        }
    }
}
