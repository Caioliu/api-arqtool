using AutoMapper;
using caiobadev_api_arqtool.DTOs;
using caiobadev_api_arqtool.Models;
using caiobadev_api_arqtool.Services.Interfaces;
using caiobadev_gmcapi.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace caiobadev_api_arqtool.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DespesasMensaisController : ControllerBase {
        private readonly IDespesaMensalService _despesaMensalService;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;


        public DespesasMensaisController(IDespesaMensalService despesaMensalService, IMapper mapper, UserManager<Usuario> userManager) {
            _despesaMensalService = despesaMensalService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetDespesasMensais() {
            try {
                var despesas = await _despesaMensalService.GetDespesasMensais();
                return Ok(new { success = true, data = despesas });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDespesaMensal(int id) {
            try {
                var despesaMensal = await _despesaMensalService.GetDespesaMensal(id);

                if (despesaMensal == null) {
                    return NotFound(new { success = false, message = "Despesa mensal não encontrada." });
                }

                return Ok(new { success = true, data = despesaMensal });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Usuario")]
        public async Task<IActionResult> GetDespesasMensaisPorUsuario() {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (user == null) {
                return NotFound(new { success = false, message = "Usuário inválido." });
            }

            try {
                var despesas = await _despesaMensalService.GetDespesasMensaisPorUsuario(user.Id);
                return Ok(new { success = true, data = despesas });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDespesaMensal(int id, DespesaMensalDto despesaMensalDto) {
            try {
                var despesaMensal = _mapper.Map<DespesaMensal>(despesaMensalDto);
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);


                if (id != despesaMensal.DespesaId) {
                    return BadRequest(new { success = false, message = "ID da despesa mensal não corresponde." });
                }

                despesaMensal.CalcularGastoAnual(); // Adicionar o cálculo do gasto anual

                await _despesaMensalService.PutDespesaMensal(id, despesaMensal);

                await _despesaMensalService.AtualizarValorTotalEPercentual(user.Id);

                return Ok(new { success = true, message = "Despesa mensal atualizada com sucesso." });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<DespesaMensal>>> PostDespesasMensais(List<DespesaMensalDto> despesasMensaisDto) {
            try {
                var despesasMensais = _mapper.Map<List<DespesaMensal>>(despesasMensaisDto);
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);

                foreach (var despesaMensal in despesasMensais) {
                    despesaMensal.CalcularGastoAnual(); // Adicionar o cálculo do gasto anual

                    await _despesaMensalService.PostDespesaMensal(despesaMensal);
                }

                await _despesaMensalService.AtualizarValorTotalEPercentual(user.Id);

                return Ok(new { success = true, message = "Despesas mensais cadastradas com sucesso." });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDespesaMensal(int id) {
            try {
                var despesaMensal = await _despesaMensalService.GetDespesaMensal(id);
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);


                if (despesaMensal == null) {
                    return NotFound(new { success = false, message = "Despesa mensal não encontrada." });
                }

                await _despesaMensalService.DeleteDespesaMensal(id);

                await _despesaMensalService.AtualizarValorTotalEPercentual(user.Id);

                return Ok(new { success = true, message = "Despesa mensal deletada com sucesso." });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

    }
}
