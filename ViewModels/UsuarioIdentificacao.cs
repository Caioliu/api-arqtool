namespace caiobadev_api_arqtool.ViewModels {
    public class UsuarioIdentificacao {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }

        public IList<string>? Perfil { get; set; }
    }
}
