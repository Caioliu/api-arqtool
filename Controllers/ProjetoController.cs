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
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;
        private readonly ApiArqtoolContext _contexto;

        public ProjetoController(IDespesaMensalService despesaMensalService, IMapper mapper, UserManager<Usuario> userManager, ApiArqtoolContext contexto) {
            _despesaMensalService = despesaMensalService;
            _mapper = mapper;
            _userManager = userManager;
            _contexto = contexto;
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
        // Endpoint para receber todas as atividades e etapas de um projeto e cadastrar no banco de dados
        [HttpPost("{projetoId}/cadastrarAtividadesEtapas")]
        public async Task<IActionResult> CadastrarAtividadesEtapas(int projetoId, [FromBody] List<Etapa> etapas) {
            try {
                var projeto = await _contexto.Projetos.FindAsync(projetoId);
                if (projeto == null) {
                    return NotFound(new { success = false, message = "Projeto não encontrado." });
                }

                projeto.Etapas = etapas; // Atribui o valor do parâmetro etapas para o projeto.Etapas
                
                // Itera sobre cada etapa recebida e adiciona ao contexto
                foreach (var etapa in etapas) {
                    // Garante que a etapa pertence ao projeto correto
                    if (etapa.ProjetoId != projetoId) {
                        return BadRequest(new { success = false, message = "A etapa não pertence ao projeto especificado." });
                    }

                    // Itera sobre cada atividade da etapa e adiciona ao contexto
                    foreach (var atividade in etapa.Atividades) {
                        atividade.EtapaId = etapa.EtapaId; // Garante que a atividade pertence à etapa correta
                        _contexto.Atividades.Add(atividade);
                    }

                    etapa.CalcularQuantidadeHoras(); // Chama a função para calcular a quantidade de horas da etapa
                    etapa.CalcularValorDaEtapa(); // Chama a função para calcular o valor da etapa

                    projeto.CalcularValorDasEtapas(); // Chama a função para calcular o valor das etapas
                    projeto.CalcularQuantidadeHoras(); // Chama a função para calcular a quantidade de horas do projeto
                    projeto.CalcularQuantidadeEtapas(); // Chama a função para calcular a quantidade de etapas do projeto
                    projeto.CalcularQuantidadeAtividades(); // Chama a função para calcular a quantidade de atividades do projeto
                    _contexto.Projetos.Update(projeto); // Atualiza o projeto no contexto
                    _contexto.Etapas.Add(etapa);
                }

                await _contexto.SaveChangesAsync();

                return Ok(new { success = true, message = "Atividades e etapas cadastradas com sucesso." });
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
                var projeto = await _contexto.Projetos.FindAsync(projetoId);
                if (projeto == null) {
                    return NotFound(new { success = false, message = "Projeto não encontrado." });
                }

                return Ok(new { success = true, data = projeto });
            } catch (Exception ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

    }
}
