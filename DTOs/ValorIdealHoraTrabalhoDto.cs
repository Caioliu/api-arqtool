using System.Text.Json.Serialization;

namespace caiobadev_api_arqtool.DTOs {
    public class ValorIdealHoraTrabalhoDto {
        public int? Id { get; set; }
        public Guid UsuarioId { get; set; }
        public decimal HorasTrabalhadasPorDia { get; set; }
        public decimal DiasTrabalhadosPorSemana { get; set; }
        public int DiasFeriasPorAno { get; set; }
        public decimal FaturamentoMensalDesejado { get; set; }
        public decimal? ReservaFinanceira { get; set; }
        [JsonIgnore]
        public IList<DespesaMensalDto>? despesaMensalDtos { get; set; }

    }
}
