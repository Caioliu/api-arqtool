namespace caiobadev_api_arqtool.Models
{
    public class Projeto
    {
        public int ProjetoId { get; set; }
        public string? Nome { get; set; }
        public string UsuarioId { get; set; }
        public ICollection<Etapa>? Etapas { get; set; }
        public double? QuantidadeHoras { get; set; }
        public double? ValorTotalDasEtapas { get; set; }
        public int? QuantidadeEtapas { get; set; }
        public int? QuantidadeAtividades { get; set; }

        public void CalcularValorDasEtapas() {
            double valorDasEtapas = 0;
                foreach (var etapa in Etapas) {
                    valorDasEtapas += etapa.ValorDaEtapa;
                }

            ValorTotalDasEtapas = valorDasEtapas;
        }

        //Função para calcular a quantidade de horas do projeto
        public void CalcularQuantidadeHoras() {
            double quantidadeHoras = 0;
                foreach (var etapa in Etapas) {
                    quantidadeHoras += etapa.QuantidadeHoras;
            }

            QuantidadeHoras = quantidadeHoras;
        }

        //Função para calcular a quantidade de etapas do projeto
        public void CalcularQuantidadeEtapas() {
            if (Etapas != null) {
                QuantidadeEtapas = Etapas.Count;
            }
        }

        //Função para calcular a quantidade de atividades do projeto
        public void CalcularQuantidadeAtividades() {
            int quantidadeAtividades = 0;
            if (Etapas != null) {
                foreach (var etapa in Etapas) {
                    quantidadeAtividades += etapa.Atividades.Count;
                }
            }

            QuantidadeAtividades = quantidadeAtividades;
        }
    }
}
