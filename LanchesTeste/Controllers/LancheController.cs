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

        public IActionResult List()
        {
            //var lanches = _lanchesRepository.Lanches;
            //return View(lanches);
            var lanchesListViewModel = new LancheListViewModel();
            lanchesListViewModel.lanches = _lanchesRepository.Lanches;
            lanchesListViewModel.CategoriaAtual = "categoria atual";

            return View(lanchesListViewModel);
		}
    }
}
