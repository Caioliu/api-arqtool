namespace caiobadev_api_arqtool.Models {
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DespesaMensal {
        [Key]
        public int DespesaId { get; set; }
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }
        [Required]
        public decimal GastoMensal { get; set; }
        public decimal? PorcentagemGastoTotal { get; set; }
        public decimal? GastoAnual { get; set; }
        public decimal? Hora { get; set; }
        public decimal? ValorTotal { get; set; }
        [Required]
        public Guid UsuarioId { get; set; }

        public void CalcularValorTotal(List<DespesaMensal> despesas) {
            ValorTotal = despesas.Sum(d => d.GastoMensal);
        }

        public void CalcularPorcentagemDoGastoTotal() {
            PorcentagemGastoTotal = (GastoMensal / ValorTotal) * 100;
        }
        public void CalcularGastoAnual() {
            GastoAnual = GastoMensal * 12;
        }
    }
}
