using System.ComponentModel.DataAnnotations;

namespace caiobadev_originalpaineis.DTOs {
    public class RegistroDTO {
        [EmailAddress]
        public string Email { get; set; }
        public string Senha { get; set; }
        [Compare("Senha", ErrorMessage = "A senha e a confirmação de senha não coincidem.")]
        public string ConfirmacaoSenha { get; set; }
        public string Nome { get; set; }
        public DateTime dataNascimento { get; set; }
        public string Telefone { get; set; }
    }
}
