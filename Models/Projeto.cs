using caiobadev_api_arqtool.Enums;
using System;
using System.Drawing;

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

        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public Urgencia? Urgencia { get; set; }
        public decimal? AcrescimoTotalUrgencia { get; set; }

        public Complexidade? Complexidade { get; set; }
        public decimal? AcrescimoTotalComplexidade { get; set; }
        public int ComplexidadeMedia { get; set; }

        public int? QuantidadeAmbientes { get; set; }
        public int? QuantidadeAmbientesMolhados { get; set; }
        public decimal? ValorTotalAmbientesMolhados { get; set; }
        public decimal? ValorTotalProjeto { get; set; }

        public void CalcularValorProjeto() {
            if (ValorTotalDasEtapas != null && AcrescimoTotalUrgencia != null && AcrescimoTotalComplexidade != null && ValorTotalAmbientesMolhados != null) {
                ValorTotalProjeto = (decimal)ValorTotalDasEtapas + AcrescimoTotalUrgencia + AcrescimoTotalComplexidade;
            }
        }

        public void CalcularValorTotalAmbientesMolhados() {
             if (ValorTotalDasEtapas != null && QuantidadeAmbientes != null && QuantidadeAmbientesMolhados != null) {
                  ValorTotalAmbientesMolhados = (decimal)ValorTotalDasEtapas * ((decimal)QuantidadeAmbientes + (decimal)QuantidadeAmbientesMolhados);
             }
        }

        public int CalcularQuantidadeDias() {
            if (DataFinal != null && DataInicial != null) {
                TimeSpan diferenca = DataFinal.Value.Subtract(DataInicial.Value);
                int quantidadeDias = diferenca.Days;
                return quantidadeDias + 1;
            }
            return 0;
        }

        public void CalcularAcrescimoTotalUrgencia() {
            int quantidadeDias = CalcularQuantidadeDias();
            
            if (quantidadeDias >= 5 && quantidadeDias <= 20) {
                Urgencia = Enums.Urgencia.ExtremamenteUrgente;
            } else if (quantidadeDias >= 21 && quantidadeDias <= 35) {
                Urgencia = Enums.Urgencia.Urgente;
            } else if (quantidadeDias >= 36 && quantidadeDias <= 50) {
                Urgencia = Enums.Urgencia.Normal;
            } else if (quantidadeDias >= 51 && quantidadeDias <= 500) {
                Urgencia = Enums.Urgencia.SemUrgencia;
            }  
            decimal valorAcrescimoUrgencia = (decimal)ValorTotalDasEtapas * (decimal)Urgencia;
            AcrescimoTotalUrgencia = valorAcrescimoUrgencia;
        }

        //Função que retorna o valor da soma de Complexidade todas as etapas de um projeto


        public void DefinirNivelComplexidade() {
            int valorTotalComplexidade = CalcularValorTotalComplexidade();

            int mediaComplexidade = valorTotalComplexidade / QuantidadeEtapas ?? 1;
            decimal acrescimoComplexidade = 0;

            if (mediaComplexidade == 1) {
                acrescimoComplexidade = 0;
            } else if (mediaComplexidade == 2) {
                acrescimoComplexidade = (decimal)0.05;
            } else if (mediaComplexidade == 3) {
                acrescimoComplexidade = (decimal)0.10; ;
            } else if (mediaComplexidade == 4) {
                acrescimoComplexidade = (decimal)0.15;
            }

            AcrescimoTotalComplexidade = (decimal)ValorTotalDasEtapas * acrescimoComplexidade;

        }

        public void CalcularAcrescimoTotalComplexidade() {
            

        }

        public void CalcularValorDasEtapas() {
            double valorDasEtapas = 0;
            foreach (var etapa in Etapas) {
                valorDasEtapas += etapa.ValorDaEtapa;
            }

            ValorTotalDasEtapas = valorDasEtapas;
        }

        public void CalcularQuantidadeHoras() {
            double quantidadeHoras = 0;
            foreach (var etapa in Etapas) {
                quantidadeHoras += etapa.QuantidadeHoras;
            }

            QuantidadeHoras = quantidadeHoras;
        }

        public void CalcularQuantidadeEtapas() {
            if (Etapas != null) {
                QuantidadeEtapas = Etapas.Count;
            }
        }

        public void CalcularQuantidadeAtividades() {
            int quantidadeAtividades = 0;
            if (Etapas != null) {
                foreach (var etapa in Etapas) {
                    quantidadeAtividades += etapa.Atividades.Count;
                }
            }

            QuantidadeAtividades = quantidadeAtividades;
        }

        public int CalcularValorTotalComplexidade() {
            int valorTotalComplexidade = 0;

            if (Etapas != null) {
                foreach (var etapa in Etapas) {
                    valorTotalComplexidade += etapa.Complexidade;
                }
            }
            ComplexidadeMedia = valorTotalComplexidade / QuantidadeEtapas ?? 1;
            return valorTotalComplexidade;
        }
    }
}
