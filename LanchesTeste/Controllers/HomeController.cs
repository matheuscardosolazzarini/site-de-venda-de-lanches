using LanchesTeste.Models;
using LanchesTeste.Repositories.Interfaces;
using LanchesTeste.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LanchesTeste.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILanchesRepository _lancersRepository;

		public HomeController(ILanchesRepository lancersRepository)
		{
			_lancersRepository = lancersRepository;
		}

		public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                LanchesPreferidos = _lancersRepository.LanchesPreferidos,
            };

            return View(homeViewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
