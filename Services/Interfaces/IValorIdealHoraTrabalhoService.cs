using caiobadev_api_arqtool.DTOs;
using caiobadev_api_arqtool.Models;

namespace caiobadev_api_arqtool.Services.Interfaces {
    public interface IValorIdealHoraTrabalhoService {
        public Task<ValorIdealHoraTrabalho> ObterPorUsuarioId(string usuarioId);
        public Task<ValorIdealHoraTrabalho> Adicionar(ValorIdealHoraTrabalhoDto valorIdealHoraTrabalhoDto, DespesaMensal despesas);
        public Task<ValorIdealHoraTrabalho> Atualizar(ValorIdealHoraTrabalhoDto valorIdealHoraTrabalhoDt, DespesaMensal despesas);
        public Task<ValorIdealHoraTrabalho> Deletar(string usuarioId);
    }
}
