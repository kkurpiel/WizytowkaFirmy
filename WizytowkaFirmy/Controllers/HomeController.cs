using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Diagnostics;
using WizytowkaFirmy.Models;
using WizytowkaFirmy.Services;

namespace WizytowkaFirmy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly EmailService emailService;

        public HomeController(ILogger<HomeController> logger, EmailService emailService)
        {
            _logger = logger;
            this.emailService = emailService;
        }

        [HttpGet]
        [Route("/NapiszdoNas")]
        public IActionResult NapiszDoNas()
        {
            return View(new NapiszDoNasModel());
        }
        [HttpPost]
        [Route("/NapiszDoNas")]
        public async Task<IActionResult> NapiszDoNas(NapiszDoNasModel napiszDoNas)
        {
            try
            {
                await emailService.WyslijEmailAsync(napiszDoNas.Email, $"Witryna Firmowa, temat: {napiszDoNas.Temat}", napiszDoNas.Tresc);
                TempData["Success"] = "Wiadomoœæ zosta³a wys³ana.";
                logger.Info($"Wiadomoœæ od: {napiszDoNas.Email}, w temacie: {napiszDoNas.Temat} zosta³a wys³ana.");
                return RedirectToAction("NapiszDoNas");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Wyst¹pi³ b³¹d podczas wysy³ania wiadomoœci.");
                logger.Error(ex.Message);
            }
            return View(NapiszDoNas());
        }
        [Route("/NaszaLokalizacja")]
        public IActionResult NaszaLokalizacja()
        {
            return View();
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
