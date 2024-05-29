using caiobadev_api_arqtool.Identity;
using caiobadev_api_arqtool.Identity.Services;
using caiobadev_api_arqtool.Models;
using caiobadev_gmcapi.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace caiobadev_api_arqtool.Context {
    public class ApiArqtoolContext : IdentityDbContext<Usuario, Perfil, string> {
        public ApiArqtoolContext(DbContextOptions<ApiArqtoolContext> options) : base(options) { }

        public DbSet<DespesaMensal> DespesasMensais { get; set; }
        public DbSet<ValorIdealHoraTrabalho> ValoresIdeaisHoraTrabalho { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Etapa> Etapas { get; set; }
        public DbSet<Atividade> Atividades { get; set; }

    }
}
