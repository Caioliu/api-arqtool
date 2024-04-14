using caiobadev_api_arqtool.Identity;

namespace caiobadev_api_arqtool.Identity.Services.Interfaces {
    public interface IUsuarioService {
        Task<bool> InserirUsuarioCliente(Usuario usuario, string senha);
        Task<bool> InserirUsuario(Usuario usuario, IEnumerable<string> perfis, string senha);
        Task<bool> VerificaUsuarioCadastrado(string nomeUsuario);
        Task<bool> Update(Usuario usuario);
        Task<IEnumerable<Usuario>> GetAll();
        Task<IEnumerable<Usuario>> GetByPerfilName(string nomePerfil);
        Task<Usuario> GetById(string usuarioId);
        Task<Usuario> GetByEmail(string email);
        Task<IList<string>> GetPerfisPorUsuario(Usuario usuario);
        Task<string> GerarTokenDeConfimacaoDeEmail(Usuario usuario);
        Task<bool> ConfirmarEmail(string email, string token);
        //Task<string> GerarTokenRedefinicaoSenha(Usuario usuario);
        //Task<bool> RedefinirSenhaComToken(Usuario usuario, string token, string novaSenha);
        Task<bool> RedefinirSenha(Usuario usuario, string novaSenha);
    }
}
