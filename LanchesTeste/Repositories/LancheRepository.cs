using LanchesTeste.Context;
using LanchesTeste.Models;
using LanchesTeste.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LanchesTeste.Repositories
{
    public class LancheRepository : ILanchesRepository

    {
        private readonly AppDbContext _context;
        public LancheRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Lanche> Lanches => _context.lanches.Include(c => c.Categoria);

        public IEnumerable<Lanche> LanchesPreferidos => _context.lanches.Where(l=> l.IsLanchePreferido).Include(c => c.Categoria);

        public Lanche GetLancheById(int lancheId)
        {
          return _context.lanches.FirstOrDefault(l=> l.LancheId == lancheId);
        }
    }
}
