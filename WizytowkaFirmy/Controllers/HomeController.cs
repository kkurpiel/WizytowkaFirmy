using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Diagnostics;
using System.Text.RegularExpressions;
using WizytowkaFirmy.Models;
using WizytowkaFirmy.Services;

namespace WizytowkaFirmy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public HomeController(ILogger<HomeController> _logger)
        {
            this._logger = _logger;
        }

        [Route("/ONas")]
        public IActionResult ONas()
        {
            return View();
        }
        [Route("/OpinieKlientow")]
        public IActionResult OpinieKlientow()
        {
            return View();
        }
        [HttpGet]
        [Route("/OpinieKlientow/admin")]
        public IActionResult OpinieKlientowAdmin()
        {
            return View();
        }
        [HttpPut]
        [Route("/OpinieKlientow/admin")]
        public async Task<IActionResult> OpinieKlientowAdmin(OpinieKlientowModel opinieKlientow)
        {
            return RedirectToAction("OpinieKlientowAdmin");
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
