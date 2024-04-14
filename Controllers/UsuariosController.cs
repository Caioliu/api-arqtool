using caiobadev_api_arqtool.Identity.Services;
using caiobadev_api_arqtool.Identity.Services.Interfaces;
using caiobadev_api_arqtool.ViewModels;
using caiobadev_api_arqtool.DTOs;
using caiobadev_api_arqtool.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace caiobadev_api_arqtool.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase {
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioLogado _usuarioLogado;
        private readonly IValidator<UsuarioCadastro> _validator;
        private readonly IValidator<EditUsuarioViewModel> _validatorEdit;
        private readonly IConfiguration _configuration;

        public UsuariosController(IUsuarioService usuarioService, IValidator<UsuarioCadastro> validator, IUsuarioLogado usuarioLogado, IValidator<EditUsuarioViewModel> validatorEdit, IConfiguration configuration) {
            _configuration = configuration;
            _usuarioService = usuarioService;
            _usuarioLogado = usuarioLogado;
            _validator = validator;
            _validatorEdit = validatorEdit;

        }


        [HttpGet("Lista")]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Get() {
            var usuarios = await _usuarioService.GetAll();

            if (!usuarios.Any()) {
                return NotFound(new { message = "Usuários não encontrados." });
            }

            var usuariosInfo = new List<UsuarioInfo>();
            for (int i = 0; i < usuarios.Count(); i++) {
                usuariosInfo.Add(new UsuarioInfo() {
                    Id = usuarios.ElementAt(i).Id,
                    Nome = usuarios.ElementAt(i).Nome,
                    Sobrenome = usuarios.ElementAt(i).Sobrenome,
                    Email = usuarios.ElementAt(i).Email,
                    Telefone = usuarios.ElementAt(i).PhoneNumber,
                    DataNascimento = usuarios.ElementAt(i).DataNascimento,
                    Perfil = await _usuarioService.GetPerfisPorUsuario(usuarios.ElementAt(i))
                });
            }

            return Ok(usuariosInfo);
        }


        [HttpGet("Cliente/Lista")]
        [Authorize]
        public async Task<IActionResult> GetClientes() {
            var usuarios = await _usuarioService.GetByPerfilName("cliente");

            if (!usuarios.Any()) {
                return NotFound(new { message = "Usuários não encontrados." });
            }

            var usuariosIdent = new List<UsuarioInfo>();
            for (int i = 0; i < usuarios.Count(); i++) {
                usuariosIdent.Add(new UsuarioInfo() {
                    Id = usuarios.ElementAt(i).Id,
                    Nome = usuarios.ElementAt(i).Nome,
                    Sobrenome = usuarios.ElementAt(i).Sobrenome,
                    Email = usuarios.ElementAt(i).Email,
                    Perfil = await _usuarioService.GetPerfisPorUsuario(usuarios.ElementAt(i))
                });
            }

            return Ok(usuariosIdent);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Get(string id) {
            var usuario = await _usuarioService.GetById(id);

            if (usuario == null) {
                return NotFound(new { message = "Usuário não encontrado." });
            }

            var usuarioInfo = new UsuarioInfo() {
                Nome = usuario.Nome,
                Sobrenome = usuario.Sobrenome,
                Email = usuario.Email,
                Telefone = usuario.PhoneNumber,
                DataNascimento = usuario.DataNascimento,
                Perfil = await _usuarioService.GetPerfisPorUsuario(usuario)
            };

            return Ok(usuarioInfo);
        }


        [HttpGet("Informacao")]
        [Authorize]
        public async Task<IActionResult> GetUsuario() {
            var usuario = await _usuarioService.GetById(_usuarioLogado.Id);

            if (usuario == null) {
                return NotFound(new { message = "Usuário não encontrado." });
            }

            var usuarioInfo = new UsuarioInfo() {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Sobrenome = usuario.Sobrenome,
                Email = usuario.Email,
                Telefone = usuario.PhoneNumber,
                DataNascimento = usuario.DataNascimento,
                Perfil = await _usuarioService.GetPerfisPorUsuario(usuario)
            };

            return Ok(usuarioInfo);
        }


        [HttpPost("Cadastro/Cliente")]
        [AllowAnonymous]
        public async Task<ActionResult> CadastrarUsuarioCliente([FromBody] UsuarioCadastro usuarioCadastro) {
            FluentValidation.Results.ValidationResult validationResult = await _validator.ValidateAsync(usuarioCadastro);

            if (!validationResult.IsValid) {
                return BadRequest(new { error = validationResult.Errors });
            }

            if (await _usuarioService.VerificaUsuarioCadastrado(usuarioCadastro.Email)) {
                return BadRequest(new { error = $"Já existe um e-mail {usuarioCadastro.Email} cadastrado." });
            }

            var usuario = new Usuario {
                Nome = usuarioCadastro.Nome,
                Sobrenome = usuarioCadastro.Sobrenome,
                UserName = usuarioCadastro.Email,
                NormalizedUserName = usuarioCadastro.Email.ToUpper(),
                Email = usuarioCadastro.Email,
                NormalizedEmail = usuarioCadastro.Email.ToUpper(),
                PhoneNumber = usuarioCadastro.Telefone,
                DataNascimento = usuarioCadastro.DataNascimento.Date
            };

            var result = await _usuarioService.InserirUsuarioCliente(usuario, usuarioCadastro.Senha);

            if (!result) return BadRequest(new { error = "Erro ao criar usuário." });

            return Ok(new { message = "Usuário criado com sucesso." });
        }


        [HttpPost("Cadastro/Especifico")]
        [Authorize(Roles = "superadmin,admin")]
        //[AllowAnonymous]
        public async Task<ActionResult> CadastrarUsuario([FromBody] UsuarioCadastro usuarioCadastro) {
            FluentValidation.Results.ValidationResult validationResult = await _validator.ValidateAsync(usuarioCadastro);

            if (!validationResult.IsValid) {
                return BadRequest(new { error = validationResult.Errors });
            }

            if (await _usuarioService.VerificaUsuarioCadastrado(usuarioCadastro.Email)) {
                return BadRequest(new { error = $"Já existe um e-mail {usuarioCadastro.Email} cadastrado." });
            }

            var usuario = new Usuario {
                Nome = usuarioCadastro.Nome,
                Sobrenome = usuarioCadastro.Sobrenome,
                UserName = usuarioCadastro.Email,
                NormalizedUserName = usuarioCadastro.Email.ToUpper(),
                Email = usuarioCadastro.Email,
                NormalizedEmail = usuarioCadastro.Email.ToUpper(),
                PhoneNumber = usuarioCadastro.Telefone,
                DataNascimento = usuarioCadastro.DataNascimento.Date
            };

            var result = await _usuarioService.InserirUsuario(usuario, usuarioCadastro.Perfil, usuarioCadastro.Senha);

            if (!result) return BadRequest(new { error = "Erro ao criar usuário." });

            return Ok(new { message = "Usuário criado com sucesso." });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "superadmin")]
        public async Task<ActionResult> Put(string id, [FromBody] EditUsuarioViewModel editUsuarioViewModel) {

            FluentValidation.Results.ValidationResult validationResult = await _validatorEdit.ValidateAsync(editUsuarioViewModel);

            if (!validationResult.IsValid) {
                return BadRequest(new { error = validationResult.Errors });
            }

            if (id != editUsuarioViewModel.Id) {
                return BadRequest(new { error = $"O id {id} informado não corresponde ao id do usuário que se deseja alterar." });
            }

            var usuario = await _usuarioService.GetById(id);

            if (usuario == null) {
                return NotFound(new { error = "Usuário não encontrado." });
            }

            usuario.Nome = editUsuarioViewModel.Nome;
            usuario.Sobrenome = editUsuarioViewModel.Sobrenome;
            usuario.PhoneNumber = editUsuarioViewModel.Telefone;
            usuario.DataNascimento = editUsuarioViewModel.DataNascimento.Date;

            var result = await _usuarioService.Update(usuario);

            if (!result) return BadRequest(new { error = "Erro ao atualizar usuário." });

            return Ok(new { message = "Usuário atualizado com sucesso.", usuario });
        }

        [HttpPost("RecuperarSenha")]
        [AllowAnonymous]
        public async Task<ActionResult> Put([FromBody] RecuperarSenhaViewModel recuperarSenhaViewModel) {
            try {
                // Verifica se as senhas digitadas são iguais.
                if (recuperarSenhaViewModel.NovaSenha != recuperarSenhaViewModel.ConfirmacaoNovaSenha) {
                    return BadRequest(new { error = "As senhas não coincidem." });
                }

                // Busca o usuário pelo e-mail
                var usuario = await _usuarioService.GetByEmail(recuperarSenhaViewModel.Email);
                if (usuario == null) {
                    return BadRequest(new { error = "Dados do usuário inválidos." });
                }

                // Verifica se os dados do usuário batem com os dados informados
                if (usuario.Email != recuperarSenhaViewModel.Email ||
                    usuario.PhoneNumber != recuperarSenhaViewModel.Telefone ||
                    usuario.DataNascimento.Date != recuperarSenhaViewModel.DataNascimento.Date) {
                    return BadRequest(new { error = "Dados do usuário inválidos." });
                }

                // Se os dados estiverem corretos, atualize a senha
                var resultado = await _usuarioService.RedefinirSenha(usuario, recuperarSenhaViewModel.NovaSenha);

                if (resultado) {
                    return Ok(new { message = "Senha atualizada com sucesso." });
                } else {
                    return BadRequest(new { error = "Falha ao Atualizar a Senha." });
                }
            } catch (Exception ex) {
                return StatusCode(500, new { error = $"Erro interno: {ex.Message}" });
            }
        }

    }
}

