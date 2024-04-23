using LanchesTeste.Models;
using LanchesTeste.Repositories.Interfaces;
using LanchesTeste.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanchesTeste.Controllers
{
	public class CarrinhoCompraController : Controller
	{

		private readonly ILanchesRepository _lanchesRepository;
		private readonly CarrinhoCompra _carrinhoCompra;

		public CarrinhoCompraController(ILanchesRepository lanchesRepository, CarrinhoCompra carrinhoCompra)
		{
			_lanchesRepository = lanchesRepository;
			_carrinhoCompra = carrinhoCompra;
		}

		public IActionResult Index()
		{
			var itens = _carrinhoCompra.GetCarrinhoCompraItems();
			_carrinhoCompra.CarrinhoCompraItems = itens;

			var carrinhoCompraVM = new CarrinhoCompraViewModel
			{
				CarrinhoCompra = _carrinhoCompra,
				CarrinhoCompraTotal = _carrinhoCompra.GetCarrinhoCompraTotal()
			};
			return View(carrinhoCompraVM);
		}

		[Authorize]
		public IActionResult AdicionarItemNoCarrinhoCompra(int lancheId)
		{
			var lancheSelecionado = _lanchesRepository.Lanches.FirstOrDefault(p => p.LancheId == lancheId);

			if (lancheSelecionado != null)
			{
				_carrinhoCompra.AdicionarAoCarrinho(lancheSelecionado);
			}

			return RedirectToAction("Index");
		}

		[Authorize]
		public IActionResult RemoverItemNoCarrinhoCompra(int lancheId)
		{
			var lancheSelecionado = _lanchesRepository.Lanches.FirstOrDefault(p => p.LancheId == lancheId);

			if (lancheSelecionado != null)
			{
				_carrinhoCompra.RemoverDoCarrinho(lancheSelecionado);
			}

			return RedirectToAction("Index");
		}
	}
}
