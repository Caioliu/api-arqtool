namespace caiobadev_api_arqtool.ViewModels {
    public class UsuarioInfo {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime DataNascimento { get; set; }

        public string Email { get; set; }
        public string Telefone { get; set; }

        public IList<string>? Perfil { get; set; }
    }
}
