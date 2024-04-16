using LanchesTeste.Context;
using LanchesTeste.Models;
using LanchesTeste.Repositories.Interfaces;

namespace LanchesTeste.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Categoria> Categorias => _context.categorias;
    }
}
