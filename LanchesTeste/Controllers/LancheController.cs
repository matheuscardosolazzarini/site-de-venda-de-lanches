using LanchesTeste.Models;
using LanchesTeste.Repositories.Interfaces;
using LanchesTeste.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LanchesTeste.Controllers
{
    public class LancheController : Controller
    {

        private readonly ILanchesRepository _lanchesRepository;

        public LancheController(ILanchesRepository lanchesRepository)
        {
            _lanchesRepository = lanchesRepository;
        }

        public IActionResult List(string categoria)
        {
            IEnumerable<Lanche> lanches;
            string categoriaAtual = string.Empty;

            if (string.IsNullOrEmpty(categoria))
            {
                lanches = _lanchesRepository.Lanches.OrderBy(l => l.LancheId);
                categoriaAtual = "Todos os lanches";
            }
            else
            {
                //if(string.Equals("Normal", categoria, StringComparison.OrdinalIgnoreCase))
                //{
                // lanches = _lanchesRepository.Lanches.Where(l => l.Categoria.CategoriaNome.Equals("Normal"))
                //.OrderBy(l => l.Nome);
                //}

                //else
                //{
                //lanches = _lanchesRepository.Lanches.Where(l => l.Categoria.CategoriaNome.Equals("Natural"))
                //   .OrderBy(l => l.Nome);
                //}

                lanches = _lanchesRepository.Lanches
                    .Where(l => l.Categoria.CategoriaNome.Equals(categoria))
                    .OrderBy(c => c.Nome);
                categoriaAtual = categoria;
            }

            var lanchesListViewModel = new LancheListViewModel
            {
                lanches = lanches,
                CategoriaAtual = categoriaAtual,
            };

            return View(lanchesListViewModel);
		}

        public IActionResult Details(int lancheId) 
        {
            var lanche= _lanchesRepository.Lanches.FirstOrDefault(l => l.LancheId == lancheId);
            return View(lanche);
        }
    }
}
