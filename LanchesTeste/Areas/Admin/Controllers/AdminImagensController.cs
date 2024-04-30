using LanchesTeste.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LanchesTeste.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminImagensController : Controller
    {
        private readonly ConfigurationImagens _myConfig;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AdminImagensController(IWebHostEnvironment hostEnvironment, IOptions<ConfigurationImagens> myConfiguration)
        {
            _hostingEnvironment = hostEnvironment;
            _myConfig = myConfiguration.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            if (files == null || files.Count == 0) 
            {
                ViewData["Erro"] = "Erro: Arquivo(s) não selecionados";
                return View(ViewData);
            }

            if (files.Count > 10) 
            {
                ViewData["Erro"] = "Erro: Quantidade de arquivos excedeu o limite!";
				return View(ViewData);
			}

            long size = files.Sum(f => f.Length);

            var filePathsName = new List<string>();

            var filepath = Path.Combine(_hostingEnvironment.WebRootPath, _myConfig.NomePastaImagensProdutos);

            foreach (var Formfile in files) 
            {
                if(Formfile.FileName.Contains(".jpg") || Formfile.FileName.Contains(".gif") || Formfile.FileName.Contains(".png"))
                {
                    var fileNameWithPath = string.Concat(filepath,"\\", Formfile.FileName);

                    filePathsName.Add(fileNameWithPath);

                    using(var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        await Formfile.CopyToAsync(stream);
                    }
                }
            }
            ViewData["Resultado"] = $"{files.Count} Arquivos foram enviados ao servidor, " + $"Com o tamanho total de : {size} bytes"; 

            ViewBag.Arquivos = filePathsName;

            return View(ViewData);
        }

        public IActionResult GetImagens()
        {
            FileManagerModel model = new FileManagerModel();

            var userImagePath = Path.Combine(_hostingEnvironment.WebRootPath, _myConfig.NomePastaImagensProdutos);

            DirectoryInfo dir = new DirectoryInfo(userImagePath);

            FileInfo[] files = dir.GetFiles();

            model.PathImagesProduto = _myConfig.NomePastaImagensProdutos;

            if(files.Length == 0)
            {
                ViewData["Erro"] = $"Nenhum arquivo foi encontrado na pasta{userImagePath}";
            }

            model.Files = files;

            return View(model);
        }

        public IActionResult DeleteFile(string fname)
        {
            string _imagemDeleta = Path.Combine(_hostingEnvironment.WebRootPath, _myConfig.NomePastaImagensProdutos + "\\", fname);


            if ((System.IO.File.Exists(_imagemDeleta)))
            {
                System.IO.File.Delete(_imagemDeleta);
                ViewData["Deletado"] = $"Arquivo(s) {_imagemDeleta} deletado com sucesso";
            }
            return View("index");
        }
    }
}
