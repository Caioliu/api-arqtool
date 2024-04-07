using System.ComponentModel.DataAnnotations;

namespace caiobadev_originalpaineis.DTOs {
    public class LoginDTO {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Senha { get; set; }
    }
}
