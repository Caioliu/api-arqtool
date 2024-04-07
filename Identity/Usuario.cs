using caiobadev_originalpaineis.DTOs;
using Microsoft.AspNetCore.Identity;

namespace caiobadev_api_arqtool.Identity {
    public class Usuario : IdentityUser {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
