using LanchesTeste.Areas.Admin.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanchesTeste.Areas.Admin.Controllers
{


    [Area("Admin")]

    public class AdminGraficoController : Controller
    {
        private readonly GraficosVendasService _graficosVendas;

        public AdminGraficoController(GraficosVendasService graficosVendas)
        {
            _graficosVendas = graficosVendas;
        }

        public JsonResult VendasLanches(int dias)
        {
            var lanchesVendasTotais = _graficosVendas.GetVendasLanches(dias);
            return Json (lanchesVendasTotais);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult VendasMensal()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VendasSemanal()
        {
            return View();
        }
    }
}
