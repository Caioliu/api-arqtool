using AutoMapper;
using caiobadev_api_arqtool.DTOs;
using caiobadev_api_arqtool.Services.Interfaces;
using caiobadev_gmcapi.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace caiobadev_api_arqtool.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ValorIdealHoraTrabalhoController : ControllerBase {
        private readonly IValorIdealHoraTrabalhoService _valorIdealHoraTrabalhoService;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;
        private readonly IDespesaMensalService _despesasMensaisService;

        public ValorIdealHoraTrabalhoController(IValorIdealHoraTrabalhoService valorIdealHoraTrabalhoService, IMapper mapper, UserManager<Usuario> userManager, IDespesaMensalService despesasMensaisService) {
            _valorIdealHoraTrabalhoService = valorIdealHoraTrabalhoService;
            _mapper = mapper;
            _userManager = userManager;
            _despesasMensaisService = despesasMensaisService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetValorIdealHoraTrabalho() {
            try {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);

                if (user == null) {
                    return NotFound(new { success = false, message = "Usuário inválido." });
                }

                var valorIdealHoraTrabalho = await _valorIdealHoraTrabalhoService.ObterPorUsuarioId(user.Id);

                if (valorIdealHoraTrabalho == null) {
                    return NotFound(new { success = false, message = "Valor ideal de hora de trabalho não encontrado." });
                }

                return Ok(new { success = true, data = valorIdealHoraTrabalho });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> PostValorIdealHoraTrabalho([FromBody] ValorIdealHoraTrabalhoDto valorIdealHoraTrabalhoDto) {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (user == null) {
                return NotFound(new { success = false, message = "Usuário inválido." });
            }

            try {
                var despesasMensais = await _despesasMensaisService.GetDespesasMensaisPorUsuario(user.Id);
                var despesaMensal = despesasMensais.FirstOrDefault();

                var valorIdealHoraTrabalho = await _valorIdealHoraTrabalhoService.Adicionar(valorIdealHoraTrabalhoDto, despesaMensal);
                return Ok(new { success = true, data = valorIdealHoraTrabalho });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public async Task<IActionResult> PutValorIdealHoraTrabalho([FromBody] ValorIdealHoraTrabalhoDto valorIdealHoraTrabalhoDto) {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (user == null) {
                return NotFound(new { success = false, message = "Usuário inválido." });
            }

            try {
                var despesasMensais = await _despesasMensaisService.GetDespesasMensaisPorUsuario(user.Id);
                var despesaMensal = despesasMensais.FirstOrDefault();

                var valorIdealHoraTrabalho = await _valorIdealHoraTrabalhoService.Atualizar(valorIdealHoraTrabalhoDto, despesaMensal);
                // Atualizar o objeto no banco de dados
              
                return Ok(new { success = true, data = valorIdealHoraTrabalho });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public async Task<IActionResult> DeleteValorIdealHoraTrabalho() {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (user == null) {
                return NotFound(new { success = false, message = "Usuário inválido." });
            }

            try {
                await _valorIdealHoraTrabalhoService.Deletar(user.Id);
                return Ok(new { success = true, message = "Valor ideal de hora de trabalho deletado com sucesso." });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

    }
}
