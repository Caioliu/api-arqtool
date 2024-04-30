using Microsoft.AspNetCore.Identity;
using System;

namespace caiobadev_gmcapi.Identity {
    public class Usuario : IdentityUser {
        public DateTime dataNascimento { get; set; }
    }
}
