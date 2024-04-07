namespace caiobadev_api_arqtool.ViewModels {
    public class UsuarioToken {
        public bool Autenticado { get; set; }
        public DateTime DataExpiracao { get; set; }
        public string Token { get; set; }
        public string Mensagem { get; set; }
    }
}
