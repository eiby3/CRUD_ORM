using CRUD_ORM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CRUD_ORM.Data
{
    public class ClienteContext : DbContext
    {
        public ClienteContext(DbContextOptions<ClienteContext> options) : base(options)
        {

        }
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<EmprestimoModel> Emprestimos { get; set; }
        public DbSet<LivroModel> Livros { get; set; }
        public DbSet<CategoriaModel> Categorias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Connection"));
        }

        
    }
}
