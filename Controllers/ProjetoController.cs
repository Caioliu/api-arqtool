using AutoMapper;
using caiobadev_api_arqtool.Context;
using caiobadev_api_arqtool.DTOs;
using caiobadev_api_arqtool.Models;
using caiobadev_api_arqtool.Services.Interfaces;
using caiobadev_gmcapi.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;

namespace caiobadev_api_arqtool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetoController : ControllerBase
    {
        private readonly IDespesaMensalService _despesaMensalService;
        private readonly IValorIdealHoraTrabalhoService _valorIdealHoraTrabalhoService;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;
        private readonly ApiArqtoolContext _contexto;

        public ProjetoController(IDespesaMensalService despesaMensalService, IMapper mapper, UserManager<Usuario> userManager, ApiArqtoolContext contexto, IValorIdealHoraTrabalhoService valorIdealHoraTrabalhoService) {
            _despesaMensalService = despesaMensalService;
            _mapper = mapper;
            _userManager = userManager;
            _contexto = contexto;
            _valorIdealHoraTrabalhoService = valorIdealHoraTrabalhoService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("{nome}")]
        public async Task<IActionResult> CadastrarProjetoEAtividadesEtapas(string nome) {
            try {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);

                var projeto = new Projeto {
                    Nome = nome,
                    UsuarioId = user.Id
                };

                // Salvar o projeto no banco de dados
                _contexto.Projetos.Add(projeto);
                await _contexto.SaveChangesAsync();

                return Ok(new { success = true, message = "Novo Projeto Cadastrado com sucesso" });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        //[Authorize(AuthenticationSchemes = "Bearer")]
        //// Endpoint para receber todas as atividades e etapas de um projeto e cadastrar no banco de dados
        //[HttpPost("{projetoId}/cadastrarAtividadesEtapas")]
        //public async Task<IActionResult> CadastrarAtividadesEtapas(int projetoId, [FromBody] Etapa etapas) {
        //    try {
        //        var projeto = await _contexto.Projetos
        //            .Include(l => l.Etapas)
        //            .FirstOrDefaultAsync(p => p.ProjetoId == projetoId);
        //        var user = await _userManager.FindByEmailAsync(User.Identity.Name);
        //        if (projeto == null) {
        //            return NotFound(new { success = false, message = "Projeto não encontrado." });
        //        }

        //        //var valorIdealHoraTrabalho = await _valorIdealHoraTrabalhoService.ObterPorUsuarioId(user.Id);
        //        var valorIdealHoraTrabalho = await _contexto.ValoresIdeaisHoraTrabalho
        //        .Where(l => l.UsuarioId == Guid.Parse(user.Id))
        //        .FirstOrDefaultAsync();

        //        foreach (var etapa in etapas) {
        //            projeto.Etapas.Add(etapa);
        //        }

        //        // Itera sobre cada etapa recebida e adiciona ao contexto
        //        foreach (var etapa in etapas) {
        //            // Garante que a etapa pertence ao projeto correto
        //            if (etapa.ProjetoId != projetoId) {
        //                return BadRequest(new { success = false, message = "A etapa não pertence ao projeto especificado." });
        //            }

        //            // Itera sobre cada atividade da etapa e adiciona ao contexto
        //            foreach (var atividade in etapa.Atividades) {
        //                atividade.EtapaId = etapa.EtapaId; // Garante que a atividade pertence à etapa correta
        //                atividade.CalcularValorAtividade(valorIdealHoraTrabalho.ValorIdealHoraDeTrabalho);
        //                _contexto.Atividades.Add(atividade);
        //            }

        //            etapa.CalcularQuantidadeHoras(); // Chama a função para calcular a quantidade de horas da etapa
        //            etapa.CalcularValorDaEtapa(); // Chama a função para calcular o valor da etapa

        //            projeto.CalcularValorDasEtapas(); // Chama a função para calcular o valor das etapas
        //            projeto.CalcularQuantidadeHoras(); // Chama a função para calcular a quantidade de horas do projeto
        //            projeto.CalcularQuantidadeEtapas(); // Chama a função para calcular a quantidade de etapas do projeto
        //            projeto.CalcularQuantidadeAtividades(); // Chama a função para calcular a quantidade de atividades do projeto
        //            projeto.DefinirNivelComplexidade();
        //            _contexto.Projetos.Update(projeto); // Atualiza o projeto no contexto
        //            _contexto.Etapas.Add(etapa);
        //        }

        //        await _contexto.SaveChangesAsync();

        //        return Ok(new { success = true, message = "Atividades e etapas cadastradas com sucesso." });
        //    } catch (Exception ex) {
        //        return BadRequest(new { success = false, message = ex.Message });
        //    }
        //}

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("{projetoId}/cadastrarEtapa")]
        public async Task<IActionResult> CadastrarEtapa(int projetoId, [FromBody] Etapa etapa) {
            try {
                var projeto = await _contexto.Projetos
                 .Include(p => p.Etapas)
                     .ThenInclude(e => e.Atividades)
                 .FirstOrDefaultAsync(p => p.ProjetoId == projetoId);


                var user = await _userManager.FindByEmailAsync(User.Identity.Name);

                if (projeto == null) {
                    return NotFound(new { success = false, message = "Projeto não encontrado." });
                }

                var valorIdealHoraTrabalho = await _contexto.ValoresIdeaisHoraTrabalho
               .Where(l => l.UsuarioId == Guid.Parse(user.Id))
               .FirstOrDefaultAsync();


                foreach (var atividade in etapa.Atividades) {
                    atividade.EtapaId = etapa.EtapaId; // Garante que a atividade pertence à etapa correta
                    atividade.CalcularValorAtividade(valorIdealHoraTrabalho.ValorIdealHoraDeTrabalho);
                    _contexto.Atividades.Add(atividade);
                }

                projeto.Etapas.Add(etapa);

                etapa.CalcularQuantidadeHoras(); // Chama a função para calcular a quantidade de horas da etapa
                etapa.CalcularValorDaEtapa(); // Chama a função para calcular o valor da etapa

                projeto.CalcularValorDasEtapas();
                projeto.CalcularQuantidadeHoras();
                projeto.CalcularQuantidadeEtapas();
                projeto.CalcularQuantidadeAtividades();
                projeto.DefinirNivelComplexidade();

                _contexto.Projetos.Update(projeto); // Atualiza o projeto no contexto
                _contexto.Etapas.Add(etapa);
                await _contexto.SaveChangesAsync();

                return Ok(new { success = true, message = "Etapa cadastrada com sucesso." });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        //Requisição para obter todas as etapas de um projeto do usuário
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{projetoId}/etapas")]
        public async Task<IActionResult> ObterEtapasDeProjeto(int projetoId) {
            try {
                var projeto = await _contexto.Projetos.FindAsync(projetoId);
                if (projeto == null) {
                    return NotFound(new { success = false, message = "Projeto não encontrado." });
                }

                var etapas = await _contexto.Etapas.Include(e => e.Atividades).Where(e => e.ProjetoId == projetoId).ToListAsync();

                return Ok(new { success = true, data = etapas });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        //Requisição para atualizar um projeto
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("calcular/{projetoId}")]
        public async Task<IActionResult> AtualizarProjeto(int projetoId, [FromBody] ProjetoInputDto projetoInput) {
            try {
                var projeto = await _contexto.Projetos
                    .Include(l => l.Etapas)
                    .FirstOrDefaultAsync(p => p.ProjetoId == projetoId);

                if (projeto == null) {
                    return NotFound(new { success = false, message = "Projeto não encontrado." });
                }

                // Atualizar os campos do projeto com os valores recebidos
                projeto.DataInicial = projetoInput.DataInicial;
                projeto.DataFinal = projetoInput.DataFinal;
                projeto.QuantidadeAmbientesMolhados = projetoInput.QuantidadeAmbientesMolhados;
                projeto.QuantidadeAmbientes = projetoInput.QuantidadeAmbientes;

                // Calcular os valores e níveis do projeto
                projeto.CalcularAcrescimoTotalUrgencia();
                projeto.CalcularValorTotalAmbientesMolhados();
                projeto.CalcularValorProjeto();         

                _contexto.Projetos.Update(projeto);
                await _contexto.SaveChangesAsync();

                return Ok(new { success = true, message = "Projeto atualizado com sucesso." });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        //Requisição GetAll Projetos
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetAllProjetos() {
            try {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);

                var projetos = await _contexto.Projetos.Where(p => p.UsuarioId == user.Id).ToListAsync();

                return Ok(new { success = true, data = projetos });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        //Requisição GetById Projeto
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{projetoId}")]
        public async Task<IActionResult> GetProjetoById(int projetoId) {
            try {
              var projeto = await _contexto.Projetos
                    .Include(l => l.Etapas)
                    .FirstOrDefaultAsync(p => p.ProjetoId == projetoId);                if (projeto == null) {
                    return NotFound(new { success = false, message = "Projeto não encontrado." });
                }

                return Ok(new { success = true, data = projeto });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        //Requisição para retornar as atividades de uma etapa pelo Id
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{etapaId}/atividades")]
        public async Task<IActionResult> ObterAtividadesDeEtapa(int etapaId) {
            try {
                var etapa = await _contexto.Etapas.Include(e => e.Atividades).FirstOrDefaultAsync(e => e.EtapaId == etapaId);
                if (etapa == null) {
                    return NotFound(new { success = false, message = "Etapa não encontrada." });
                }

                return Ok(new { success = true, data = etapa.Atividades });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        //Excluir atividade pelo Id
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("atividade/{atividadeId}")]
        public async Task<IActionResult> ExcluirAtividade(int atividadeId) {
            try {
                var atividade = await _contexto.Atividades.FindAsync(atividadeId);
                if (atividade == null) {
                    return NotFound(new { success = false, message = "Atividade não encontrada." });
                }

                _contexto.Atividades.Remove(atividade);
                await _contexto.SaveChangesAsync();

                return Ok(new { success = true, message = "Atividade excluída com sucesso." });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        //Excluir etapa pelo Id
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("etapa/{etapaId}")]
        public async Task<IActionResult> ExcluirEtapa(int etapaId) {
            try {
                var etapa = await _contexto.Etapas.FindAsync(etapaId);
                if (etapa == null) {
                    return NotFound(new { success = false, message = "Etapa não encontrada." });
                }

                _contexto.Etapas.Remove(etapa);
                await _contexto.SaveChangesAsync();

                return Ok(new { success = true, message = "Etapa excluída com sucesso." });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        //Excluir projeto pelo Id
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{projetoId}")]
        public async Task<IActionResult> ExcluirProjeto(int projetoId) {
            try {
                var projeto = await _contexto.Projetos.FindAsync(projetoId);
                if (projeto == null) {
                    return NotFound(new { success = false, message = "Projeto não encontrado." });
                }

                _contexto.Projetos.Remove(projeto);
                await _contexto.SaveChangesAsync();

                return Ok(new { success = true, message = "Projeto excluído com sucesso." });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


    }
}
