using LanchesTeste.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LanchesTeste.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        { 
        }

        public DbSet<Categoria> categorias { get; set; }
        public DbSet<Lanche> lanches { get; set; } 
        public DbSet<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalhe> PedidoDetalhes { get; set; }

	}
}
