using caiobadev_api_arqtool.Identity;
using caiobadev_api_arqtool.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace caiobadev_api_arqtool.Identity.Services {
    public class UsuarioService : IUsuarioService {

        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;



        public UsuarioService(SignInManager<Usuario> signInManager, UserManager<Usuario> userManager) {
            _signInManager = signInManager;
            _userManager = userManager;
        }



        public async Task<IEnumerable<Usuario>> GetAll() {
            return await _userManager.Users.AsNoTracking().ToListAsync();
        }


        public async Task<IEnumerable<Usuario>> GetByPerfilName(string nomePerfil) {
            return await _userManager.GetUsersInRoleAsync(nomePerfil);
        }


        public async Task<Usuario> GetById(string usuarioId) {
            //return await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == usuarioId);
            return await _userManager.FindByIdAsync(usuarioId);
        }


        public async Task<Usuario> GetByEmail(string email) {
            return await _userManager.FindByEmailAsync(email);
        }


        public async Task<IList<string>> GetPerfisPorUsuario(Usuario usuario) {
            return await _userManager.GetRolesAsync(usuario);
        }


        public async Task<bool> InserirUsuarioCliente(Usuario usuario, string senha) {
            var resultado = await _userManager.CreateAsync(usuario, senha);

            if (resultado.Succeeded) {
                List<string> perfis = new()
                {
                    "cliente"
                };

                await _userManager.AddToRolesAsync(usuario, perfis);
            }

            return resultado.Succeeded;
        }


        public async Task<bool> InserirUsuario(Usuario usuario, IEnumerable<string> perfis, string senha) {
            var resultado = await _userManager.CreateAsync(usuario, senha);

            if (resultado.Succeeded) {

                await _userManager.AddToRolesAsync(usuario, perfis);
            }

            return resultado.Succeeded;
        }

        public async Task<bool> Update(Usuario usuario) {
            var resultado = await _userManager.UpdateAsync(usuario);

            return resultado.Succeeded;
        }


        public async Task<bool> VerificaUsuarioCadastrado(string nomeUsuario) {
            if (await _userManager.FindByNameAsync(nomeUsuario) != null) {
                return true;
            }

            return false;
        }


        public async Task<string> GerarTokenDeConfimacaoDeEmail(Usuario usuario) {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(usuario);

            return token;
        }


        public async Task<bool> ConfirmarEmail(string email, string token) {
            var usuario = await _userManager.FindByEmailAsync(email);
            if (usuario == null) return false;

            var result = await _userManager.ConfirmEmailAsync(usuario, token);

            if (result.Succeeded) {
                return true;
            } else {
                return false;
            }
        }

        public async Task<bool> RedefinirSenha(Usuario usuario, string novaSenha) {
            var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
            var result = await _userManager.ResetPasswordAsync(usuario, token, novaSenha);

            if (result.Succeeded) {
                return true;
            } else {
                return false;
            }
        }

        //public async Task<string> GerarTokenRedefinicaoSenha(Usuario usuario) {
        //    // Gere o token de redefinição de senha para o usuário
        //    var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
        //    return token;
        //}

        //public async Task<bool> AtualizarSenhaComToken(Usuario usuario, string token, string novaSenha) {
        //    // Verifique se o token é válido para o usuário
        //    var resultadoVerificacao = await _userManager.ResetPasswordAsync(usuario, token, novaSenha);
        //    if (!resultadoVerificacao.Succeeded) {
        //        // O token é inválido ou expirou
        //        return false;
        //    }

        //    // Atualize a senha do usuário
        //    var resultadoAtualizacao = await _userManager.ChangePasswordAsync(usuario, null, novaSenha);
        //    return resultadoAtualizacao.Succeeded;
        //}


    }
}
