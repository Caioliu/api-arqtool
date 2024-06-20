namespace caiobadev_api_arqtool.DTOs
{
    public class ProjetoInputDto
    {
        public int ProjetoId { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public int? QuantidadeAmbientes { get; set; }
        public int? QuantidadeAmbientesMolhados { get; set; }
    }
}
