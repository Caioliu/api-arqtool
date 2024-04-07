using caiobadev_api_arqtool.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace caiobadev_api_arqtool.Identity.Services {
    public class PerfilService : IPerfilService {
        private readonly RoleManager<Perfil> _roleManager;

        public PerfilService(RoleManager<Perfil> roleManager) {
            _roleManager = roleManager;
        }


        public async Task<bool> Insert(string nomePerfil) {
            var perfil = new Perfil {
                Name = nomePerfil,
                NormalizedName = nomePerfil.ToLower()
            };

            var resultado = await _roleManager.CreateAsync(perfil);

            return resultado.Succeeded;
        }


        public async Task<bool> Update(Perfil perfil) {

            var resultado = await _roleManager.UpdateAsync(perfil);

            return resultado.Succeeded;
        }


        public async Task<bool> Delete(Perfil perfil) {
            var resultado = await _roleManager.DeleteAsync(perfil);

            return resultado.Succeeded;
        }


        public async Task<Perfil> GetById(string perfilId) {
            return await _roleManager.FindByIdAsync(perfilId);
        }


        public async Task<Perfil> GetByName(string nomePerfil) {
            return await _roleManager.FindByNameAsync(nomePerfil);
        }


        public IEnumerable<Perfil> GetAll() {
            return _roleManager.Roles;
        }
    }
}
