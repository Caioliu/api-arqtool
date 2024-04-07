using caiobadev_api_arqtool.ApiArqtool.Domain.Interfaces.Account;
using caiobadev_api_arqtool.Identity;
using Microsoft.AspNetCore.Identity;
namespace caiobadev_api_arqtool.Identity.Services {
    public class AutenticacaoService : IAutenticacaoService {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;



        public AutenticacaoService(SignInManager<Usuario> signInManager, UserManager<Usuario> userManager) {
            _signInManager = signInManager;
            _userManager = userManager;
        }



        public async Task<bool> AutenticarUsuario(string nomeUsuario, string senha) {
            var resultado = await _signInManager.PasswordSignInAsync(nomeUsuario, senha, isPersistent: false, lockoutOnFailure: false);
            return resultado.Succeeded;
        }


        public async Task<IList<string>> GetPerfilUsuario(string nomeUsuario) {
            var usuario = await _userManager.FindByNameAsync(nomeUsuario);

            return await _userManager.GetRolesAsync(usuario);
        }


        public async Task<string> GerarTokenDeConfimacaoDeEmail(string email) {
            var usuario = await _userManager.FindByEmailAsync(email);

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(usuario);

            return token;
        }


        public async Task<bool> VerificaEmailConfirmado(string email) {
            var usuario = await _userManager.FindByEmailAsync(email);
            if (usuario == null) {
                return false;
            }

            return await _userManager.IsEmailConfirmedAsync(usuario);
        }


        public async Task Logout() {
            await _signInManager.SignOutAsync();
        }

    }
}
