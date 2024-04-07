using System.ComponentModel.DataAnnotations;

namespace caiobadev_originalpaineis.DTOs {
    public enum TipoUsuario {
        Cliente,
        Administrador,
        Superadmin
    }

    public class RegistroDTO {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Senha { get; set; }

        [Required]
        [Compare("Senha", ErrorMessage = "A senha e a confirmação de senha não coincidem.")]
        public string? ConfirmacaoSenha { get; set; }
    }
}
