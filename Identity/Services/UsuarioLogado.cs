using caiobadev_api_arqtool.Identity;
using caiobadev_api_arqtool.Identity.Services.Interfaces;
using caiobadev_gmcapi.Identity;
using Microsoft.AspNetCore.Identity;

namespace caiobadev_api_arqtool.Identity.Services {
    public class UsuarioLogado : IUsuarioLogado {
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<Usuario> _userManager;

        public UsuarioLogado(IHttpContextAccessor accessor, UserManager<Usuario> userManager) {
            _accessor = accessor;
            _userManager = userManager;
        }

        public string NomeUsuario => _accessor.HttpContext.User.Identity.Name;
        public string Id => _userManager.FindByNameAsync(NomeUsuario).Result.Id;


        public bool IsAuthenticated() {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
