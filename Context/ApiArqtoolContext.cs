using Microsoft.EntityFrameworkCore;

namespace caiobadev_api_arqtool.Context {
    public class ApiArqtoolContext : DbContext {
        public ApiArqtoolContext(DbContextOptions<ApiArqtoolContext> options) : base(options) { }
        
    }
}
