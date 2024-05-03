namespace caiobadev_api_arqtool.DTOs {
    public class DespesaMensalDto {
        public int? DespesaId { get; set; }
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; }
        public decimal GastoMensal { get; set; }
        
    }
}
