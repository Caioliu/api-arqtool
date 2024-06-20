using caiobadev_api_arqtool.Models;

namespace caiobadev_api_arqtool.Services.Interfaces {
    public interface IDespesaMensalService {
        public Task<IEnumerable<DespesaMensal>> GetDespesasMensais();
        public Task<DespesaMensal> GetDespesaMensal(int id);
        public Task PutDespesaMensal(int id, DespesaMensal despesaMensal);
        public Task PostDespesaMensal(DespesaMensal despesaMensal);
        public Task DeleteDespesaMensal(int id);
        public Task AtualizarValorTotalEPercentual(string usuarioId);
        public Task<IEnumerable<DespesaMensal>> GetDespesasMensaisPorUsuario(string usuarioId);
    }
}
