using caiobadev_api_arqtool.Context;
using caiobadev_api_arqtool.Models;
using caiobadev_api_arqtool.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace caiobadev_api_arqtool.Services {
    public class DespesaMensalService : IDespesaMensalService {
        private readonly ApiArqtoolContext _context;

        public DespesaMensalService(ApiArqtoolContext context) {
            _context = context;
        }

        public async Task<IEnumerable<DespesaMensal>> GetDespesasMensais() {
            return await _context.DespesasMensais.ToListAsync();
        }

        public async Task<DespesaMensal> GetDespesaMensal(int id) {
            return await _context.DespesasMensais.FindAsync(id);
        }

       public async Task PutDespesaMensal(int id, DespesaMensal despesaMensal) {
            _context.Entry(despesaMensal).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task PostDespesaMensal(DespesaMensal despesaMensal) {
            _context.DespesasMensais.Add(despesaMensal);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteDespesaMensal(int id) {
            var despesaMensal = await _context.DespesasMensais.FindAsync(id);

            _context.DespesasMensais.Remove(despesaMensal);

            await _context.SaveChangesAsync();
        }
        public async Task AtualizarValorTotalEPercentual(string usuarioId) {
            var despesasMensais = await GetDespesasMensaisPorUsuario(usuarioId);
            foreach (var despesaMensal in despesasMensais) {
                despesaMensal.CalcularValorTotal(despesasMensais.ToList());
                despesaMensal.CalcularPorcentagemDoGastoTotal();
                await PutDespesaMensal(despesaMensal.DespesaId, despesaMensal);
            }
        }

        public async Task<IEnumerable<DespesaMensal>> GetDespesasMensaisPorUsuario(string usuarioId) {
            return await _context.DespesasMensais.Where(d => d.UsuarioId == Guid.Parse(usuarioId)).ToListAsync();
        }
    }
}
