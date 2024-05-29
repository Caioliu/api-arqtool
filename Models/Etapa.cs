namespace caiobadev_api_arqtool.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;
    using System.Text.Json.Serialization;

    public class Etapa
    {
        [Key]
        public int EtapaId { get; set; }
        public int ProjetoId { get; set; }
        [JsonIgnore]
        public Projeto? Projeto { get; set; }
        public ICollection<Atividade> Atividades { get; set; }
        public int Complexidade { get; set; }
        public double ValorDaEtapa { get; set; }
        public double QuantidadeHoras { get; set; }

        //Função para calcular a quantidade de horas da etapa
        public void CalcularQuantidadeHoras() {
            double quantidadeHoras = 0;
            foreach (var atividade in Atividades) {
                quantidadeHoras += atividade.DuracaoEmHoras;
            }

            QuantidadeHoras = quantidadeHoras;
        }
        //Função para calcular o valor da etapa
        public void CalcularValorDaEtapa() {
            double valorEtapas = 0;
            foreach (var atividade in Atividades) {
                valorEtapas += CalcularAcrescimoComplexidade(atividade.Valor);
            }

            ValorDaEtapa = valorEtapas;
        }

        //Função para calcular o acréscimo de complexidade, se complexidade for 1, não haverá acréscimo, se for 2, haverá um acréscimo de 5% e se for 3, haverá um acréscimo de 15%, se for 4, haverá um acréscimo de 20%
        public double CalcularAcrescimoComplexidade(double Valor) {
            if (Complexidade == 2) { return Valor + Valor * 0.05; }
            if (Complexidade == 3) { return Valor + Valor * 0.10; }
            if (Complexidade == 4) { return Valor + Valor * 0.15; }
            return 0;
        }
    }
}
