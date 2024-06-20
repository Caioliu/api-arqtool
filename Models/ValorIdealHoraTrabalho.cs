namespace caiobadev_api_arqtool.Models {
    public class ValorIdealHoraTrabalho {
        public int Id { get; set; }
        public Guid UsuarioId { get; set; }
        public decimal FaturamentoMensalDesejado { get; set; }
        public decimal? ReservaFinanceira { get; set; }
        public decimal? TotalDespesasMensais { get; set; }
        public decimal HorasTrabalhadasPorDia { get; set; }
        public decimal DiasTrabalhadosPorSemana { get; set; }
        public decimal? DiasTrabalhadosPorMes { get; set; }
        public decimal? HorasTrabalhadasPorMes { get; set; }
        public int DiasFeriasPorAno { get; set; }
        public decimal? FaturamentoMensalMinimo { get; set; }
        public decimal? ValorMinimoHoraDeTrabalho { get; set; }
        public decimal? ValorIdealHoraDeTrabalho { get; set; }
        public decimal? CustoFerias { get; set; }
        public decimal? PercentualCustoFerias { get; set; }
        public decimal? PercentualReservaFerias { get; set; }

        public void CalcularDiasTrabalhadosPorMes() {
            if (DiasTrabalhadosPorSemana == 5) {
                DiasTrabalhadosPorMes = (decimal) 21.17;
            } else {
                DiasTrabalhadosPorMes = DiasTrabalhadosPorSemana * 4;
            }
        }

        public void CalcularHorasTrabalhadasPorMes() {
                HorasTrabalhadasPorMes = HorasTrabalhadasPorDia * DiasTrabalhadosPorMes.Value;
            }

        public void ObterTotalDespesasMensais(DespesaMensal despesas) {
            TotalDespesasMensais = despesas.ValorTotal;
        }

        public void CalcularCustoFerias() {
            CustoFerias = (FaturamentoMensalDesejado + TotalDespesasMensais) * DiasFeriasPorAno / 30;
        }

        public void CalcularPercentualCustoFerias() {
            PercentualCustoFerias = CustoFerias / (FaturamentoMensalDesejado + TotalDespesasMensais);
        }

        public void CalcularPercentualReservaFerias() {
            PercentualReservaFerias = PercentualCustoFerias / 11;
        }

        public void CalcularFaturamentoMensalMinimo() {
            FaturamentoMensalMinimo = (PercentualReservaFerias * (FaturamentoMensalDesejado + ReservaFinanceira + TotalDespesasMensais)) + (FaturamentoMensalDesejado + ReservaFinanceira + TotalDespesasMensais);
        }

        public void CalcularValorIdealHoraDeTrabalho() {
            ValorIdealHoraDeTrabalho = FaturamentoMensalMinimo / HorasTrabalhadasPorMes.Value;
        }

        public void CalcularValorMinimoHoraDeTrabalho() {
            ValorMinimoHoraDeTrabalho = (FaturamentoMensalDesejado + TotalDespesasMensais + ReservaFinanceira) / HorasTrabalhadasPorMes;
        }

        }
    }
