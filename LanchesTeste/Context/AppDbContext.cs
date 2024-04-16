using LanchesTeste.Models;
using Microsoft.EntityFrameworkCore;

namespace LanchesTeste.Context
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        { 
        }

        public DbSet<Categoria> categorias { get; set; }
        public DbSet<Lanche> lanches { get; set; } 
        public DbSet<CarrinhoCompraItem> CarrinhoCompraItems { get; set; }

	}
}
