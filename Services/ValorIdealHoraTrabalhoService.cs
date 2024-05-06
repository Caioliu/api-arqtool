using AutoMapper;
using caiobadev_api_arqtool.Context;
using caiobadev_api_arqtool.DTOs;
using caiobadev_api_arqtool.DTOs.Mappings;
using caiobadev_api_arqtool.Models;
using caiobadev_api_arqtool.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace caiobadev_api_arqtool.Services {
    public class ValorIdealHoraTrabalhoService : IValorIdealHoraTrabalhoService {
        private readonly ApiArqtoolContext _context;
        private readonly IDespesaMensalService _despesasMensaisService;

        public ValorIdealHoraTrabalhoService(ApiArqtoolContext context, IDespesaMensalService despesasMensaisService) {
            _context = context;
            _despesasMensaisService = despesasMensaisService;
        }

        public async Task<ValorIdealHoraTrabalho> ObterPorUsuarioId(string usuarioId) {
            var valorIdealHoraTrabalho = _context.ValoresIdeaisHoraTrabalho.FirstOrDefault(v => v.UsuarioId == Guid.Parse(usuarioId));

            if (valorIdealHoraTrabalho == null) { 
                return null;
            }

            var despesasMensais = await _despesasMensaisService.GetDespesasMensaisPorUsuario(usuarioId);
            var despesaMensal = despesasMensais.FirstOrDefault();

            if (valorIdealHoraTrabalho.TotalDespesasMensais != despesaMensal.ValorTotal) {
                CalcularValores(valorIdealHoraTrabalho, despesaMensal);
                _context.Update(valorIdealHoraTrabalho);
                await _context.SaveChangesAsync();
            }

            return await Task.FromResult(valorIdealHoraTrabalho);
        }

        public async Task<ValorIdealHoraTrabalho> Adicionar(ValorIdealHoraTrabalhoDto valorIdealHoraTrabalhoDto, DespesaMensal despesas) {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);

            var valorIdealHoraTrabalho = mapper.Map<ValorIdealHoraTrabalho>(valorIdealHoraTrabalhoDto);

            CalcularValores(valorIdealHoraTrabalho, despesas);

            // Adicionar o objeto no banco de dados
            _context.ValoresIdeaisHoraTrabalho.Add(valorIdealHoraTrabalho);
            await _context.SaveChangesAsync();

            return await Task.FromResult(valorIdealHoraTrabalho);
        }

        public async Task<ValorIdealHoraTrabalho> Atualizar(ValorIdealHoraTrabalhoDto valorIdealHoraTrabalhoDto, DespesaMensal despesas) {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);

            var valorIdealHoraTrabalho = mapper.Map<ValorIdealHoraTrabalho>(valorIdealHoraTrabalhoDto);

            CalcularValores(valorIdealHoraTrabalho, despesas);

            var existingValorIdealHoraTrabalho = await _context.ValoresIdeaisHoraTrabalho.FindAsync(valorIdealHoraTrabalhoDto.Id);

            if (existingValorIdealHoraTrabalho != null) {
                _context.Entry(existingValorIdealHoraTrabalho).CurrentValues.SetValues(valorIdealHoraTrabalho);
                await _context.SaveChangesAsync();
            }

            return await Task.FromResult(valorIdealHoraTrabalho);
        }

        public async Task<ValorIdealHoraTrabalho> Deletar(string usuarioId) {
            var valorIdealHoraTrabalho = await _context.ValoresIdeaisHoraTrabalho.FirstOrDefaultAsync(v => v.UsuarioId == Guid.Parse(usuarioId));

            if (valorIdealHoraTrabalho != null) {
                _context.ValoresIdeaisHoraTrabalho.Remove(valorIdealHoraTrabalho);
                await _context.SaveChangesAsync();
            }

            return valorIdealHoraTrabalho;
        }

        private void CalcularValores(ValorIdealHoraTrabalho valorIdealHoraTrabalho, DespesaMensal despesas) {
            valorIdealHoraTrabalho.CalcularDiasTrabalhadosPorMes();
            valorIdealHoraTrabalho.CalcularHorasTrabalhadasPorMes();
            valorIdealHoraTrabalho.ObterTotalDespesasMensais(despesas);
            valorIdealHoraTrabalho.CalcularCustoFerias();
            valorIdealHoraTrabalho.CalcularPercentualCustoFerias();
            valorIdealHoraTrabalho.CalcularPercentualReservaFerias();
            valorIdealHoraTrabalho.CalcularFaturamentoMensalMinimo();
            valorIdealHoraTrabalho.CalcularValorIdealHoraDeTrabalho();
            valorIdealHoraTrabalho.CalcularValorMinimoHoraDeTrabalho();
        }
    }
}
