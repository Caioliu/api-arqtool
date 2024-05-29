namespace caiobadev_api_arqtool.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class Atividade
    {
        [Key]
        public int AtividadeId { get; set; }
        public string Nome { get; set; }
        public double DuracaoEmHoras { get; set; }
        public double Valor { get; set; }
        public int EtapaId { get; set; }
        [JsonIgnore]
        public Etapa? Etapa { get; set; }

    }
}
