namespace caiobadev_api_arqtool.ApiArqtool.Domain.Interfaces.Account;
public interface IAutenticacaoService {
    Task<bool> AutenticarUsuario(string userName, string senha);
    Task Logout();
    Task<IList<string>> GetPerfilUsuario(string nomeUsuario);
    Task<bool> VerificaEmailConfirmado(string email);
    Task<string> GerarTokenDeConfimacaoDeEmail(string email);
}
