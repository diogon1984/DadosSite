using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System;
using DadosSite.Negocio;

namespace DadosSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUrlNegocio _urlNegocio;

        public HomeController(IUrlNegocio urlNegocio)
        {
            _urlNegocio = urlNegocio;
        }

        public IActionResult Index()
        {   
            return View();
        }

        public IActionResult Listar(string url, int ranking)
        {
            try
            {
                var html = new HtmlWeb().Load($"http://{url}");

                @ViewBag.Imagens = _urlNegocio.GetImagens(html);
                @ViewBag.Ranking = _urlNegocio.RankingPalavras(html, ranking);
            }
            catch (Exception)
            {
                @ViewBag.MsgErro = "URL inválida!!";
            }

            return View("Index");
        }
    }
}
