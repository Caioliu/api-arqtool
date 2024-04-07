using caiobadev_api_arqtool.ApiArqtool.Domain.Interfaces.Account;
using caiobadev_api_arqtool.Identity;
using Microsoft.AspNetCore.Identity;


namespace caiobadev_api_arqtool.Identity.Services {
    public class GeracaoUsuariosPerfisIniciais : IGeracaoUsuariosPerfisIniciais {

        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<Perfil> _roleManager;

        public GeracaoUsuariosPerfisIniciais(RoleManager<Perfil> roleManager,
              UserManager<Usuario> userManager) {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void GerarPerfis() {
            string[] perfis = { "cliente", "admin", "superadmin" };

            foreach (string perfilNome in perfis) {
                if (!_roleManager.RoleExistsAsync(perfilNome).Result) {
                    Perfil perfil = new Perfil {
                        Name = perfilNome,
                        NormalizedName = perfilNome.ToUpper()
                    };

                    IdentityResult roleResult = _roleManager.CreateAsync(perfil).Result;
                }
            }
        }



        public void GerarUsuarios() {
            //if (_userManager.FindByNameAsync("email2023@yahoo.com.br").Result == null)
            //{
            //    Usuario usuario = new();
            //    usuario.UserName = "email2023@yahoo.com.br";
            //    usuario.NormalizedUserName = usuario.UserName.ToUpper();
            //    usuario.Email = usuario.UserName;
            //    usuario.NormalizedEmail = usuario.Email.ToUpper();
            //    usuario.LockoutEnabled = false;
            //    usuario.SecurityStamp = Guid.NewGuid().ToString();
            //    usuario.Nome = "Usuário";
            //    usuario.Sobrenome = "da Silva";
            //    usuario.PhoneNumber = "38123456789";
            //    usuario.DataNascimento = Convert.ToDateTime("01/01/1901").Date.ToUniversalTime();

            //    IdentityResult result = _userManager.CreateAsync(usuario, "O2x8Wqcz9TE$").Result;

            //    if (result.Succeeded)
            //    {
            //        _userManager.AddToRoleAsync(usuario, "superadmin").Wait();
            //    }
            //}
        }

    }
}
