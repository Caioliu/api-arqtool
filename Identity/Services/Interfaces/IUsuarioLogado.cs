namespace caiobadev_api_arqtool.Identity.Services.Interfaces {
    public interface IUsuarioLogado {
        string NomeUsuario { get; }
        string Id { get; }
        bool IsAuthenticated();
    }
}
