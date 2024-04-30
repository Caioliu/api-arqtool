using caiobadev_api_arqtool.Identity;
using caiobadev_gmcapi.Identity;
using caiobadev_originalpaineis.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace caiobadev_api_arqtool.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IConfiguration _configuration;

        public UsuariosController(UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            IConfiguration configuration) {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Lista")]
        public ActionResult<IEnumerable<object>> ObterListaUsuarios() {
            var usuarios = _userManager.Users.Select(user => new { user.Id, user.Email, Senha = (string)null }).ToList();
            return Ok(usuarios);
        }

        
        [HttpPost("/api/v1/Usuario/Registro")]
        public async Task<ActionResult> RegisterUser([FromBody] RegistroDTO model) {
            var user = new Usuario {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                PhoneNumber = model.Telefone,
                dataNascimento = model.dataNascimento
            };

            var result = await _userManager.CreateAsync(user, model.Senha);

            if (!result.Succeeded) {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, false);
            return Ok("Conta criada com sucesso!");
        }

        [HttpPost("/api/v1/Usuario/Login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO userInfo) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(userInfo.Email);

            if (user == null) {
                ModelState.AddModelError(string.Empty, "Usuário não encontrado.");
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(user, userInfo.Senha, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded) {
                return Ok(GeraToken(userInfo));
            } else {
                ModelState.AddModelError(string.Empty, "Login Inválido.");
                return BadRequest(ModelState);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("/api/v1/Usuario/Info")]
        public async Task<ActionResult> UserInfo() {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (user == null) {
                return NotFound();
            }

            return Ok(new {
                user.Id,
                user.Email,
                user.PhoneNumber,
                user.dataNascimento
            });
        }

        private UsuarioToken GeraToken(LoginDTO userInfo) {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("meuPet", "cristal"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));

            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credenciais);

            return new UsuarioToken() {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT OK"
            };
        }

    }
}
