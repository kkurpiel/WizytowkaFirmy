using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Diagnostics;
using System.Text.RegularExpressions;
using WizytowkaFirmy.Models;
using WizytowkaFirmy.Services;

namespace WizytowkaFirmy.Controllers
{
    public class NapiszDoNasController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<NapiszDoNasController> _logger;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly EmailService emailService;
        private GoogleService googleService;
        public NapiszDoNasController(ILogger<NapiszDoNasController> _logger, IConfiguration configuration, EmailService emailService, GoogleService googleService)
        {
            this._logger = _logger;
            this.configuration = configuration;
            this.emailService = emailService;
            this.googleService = googleService;
        }

        [HttpGet]
        [Route("/NapiszdoNas")]
        public IActionResult NapiszDoNas()
        {
            return View(new NapiszDoNasModel());
        }

        [HttpPost]
        [Route("/NapiszDoNas")]
        public async Task<IActionResult> NapiszDoNas(NapiszDoNasModel napiszDoNas, string RecaptchaResponse)
        {
            try
            {
                var config = configuration.GetSection("reCAPTCHA");
                if (!await googleService.WeryfikacjareCAPTCHA(RecaptchaResponse, config["ServerKey"]))
                {
                    ModelState.AddModelError("", "Weryfikacja reCAPTCHA zakończyła się niepowodzeniem.");
                    logger.Error("Weryfikacja reCAPTCHA zakończyła się niepowodzeniem.");
                    return View(napiszDoNas);
                }
                if (string.IsNullOrEmpty(napiszDoNas.Email) || string.IsNullOrEmpty(napiszDoNas.Temat) || string.IsNullOrEmpty(napiszDoNas.Tresc))
                {
                    ModelState.AddModelError("", "Uzupełnij e-mail, temat lub treść wiadomości.");
                    logger.Error("Brak emaila, tematu lub treści wiadomości.");
                    return View(napiszDoNas);
                }
                if (!Regex.IsMatch(napiszDoNas.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    ModelState.AddModelError("Email", "Niepoprawny adres email.");
                    logger.Error($"Błędny adres email: {napiszDoNas.Email}");
                    return View(napiszDoNas);
                }
                await emailService.WyslijEmailAsync(napiszDoNas.Email, $"Witryna Firmowa, temat: {napiszDoNas.Temat}", napiszDoNas.Tresc);

                TempData["WyslanoEmail"] = "Wiadomość została wysłana.";
                logger.Info($"Wiadomość od: {napiszDoNas.Email}, w temacie: {napiszDoNas.Temat} została wysłana");
                return RedirectToAction("NapiszDoNas");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Wystąpił błąd podczas wysyłania wiadomości.");
                logger.Error(ex.Message);
                return View(napiszDoNas);
            }
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
