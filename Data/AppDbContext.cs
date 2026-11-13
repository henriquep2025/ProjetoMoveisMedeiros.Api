using Microsoft.EntityFrameworkCore;
using ProjetoMoveisMedeiros.Api.Models;

namespace ProjetoMoveisMedeiros.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Obra> Obras { get; set; }
        public DbSet<Pendencia> Pendencias { get; set; }
    }
}