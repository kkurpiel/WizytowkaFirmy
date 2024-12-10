using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using System.Text.RegularExpressions;
using WizytowkaFirmy.Models;
using WizytowkaFirmy.Models.DbModels;
using WizytowkaFirmy.Services;

namespace WizytowkaFirmy.Controllers
{
    public class NapiszDoNasController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IConfiguration configuration;
        private readonly EmailService emailService;
        private RecaptchaService recaptchaService;
        private DbService dbService;
        public NapiszDoNasController(IConfiguration configuration, EmailService emailService, RecaptchaService recaptchaService, DbService dbService)
        {
            this.configuration = configuration;
            this.emailService = emailService;
            this.recaptchaService = recaptchaService;
            this.dbService = dbService;
        }
        
        /// <summary>
        /// Wyświetl formularz pozwalający na przesłanie wiadomości e-mail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/napisz-do-nas")]
        public IActionResult NapiszDoNas()
        {
            try
            {
                return View(new WiadomoscEmail());
            }
            catch (Exception ex)
            {
                logger.Error($"Błąd podczas ładowania formularza wysyłania wiadomości e-mail: {ex.Message}.");
                return RedirectToAction("");
            }
        }

        /// <summary>
        /// Wyślij wiadomość e-mail
        /// </summary>
        /// <param name="wiadomosc"></param>
        /// <param name="RecaptchaResponse"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/napisz-do-nas")]
        public async Task<IActionResult> NapiszDoNas(WiadomoscEmail wiadomosc, string RecaptchaResponse)
        {
            try
            {
                var config = configuration.GetSection("reCAPTCHA");
                if (!await recaptchaService.WeryfikacjareCAPTCHA(RecaptchaResponse, config["SecretKey"]))
                {
                    ModelState.AddModelError("", "Weryfikacja reCAPTCHA zakończyła się niepowodzeniem.");
                    logger.Error("Weryfikacja reCAPTCHA zakończyła się niepowodzeniem.");
                    return View(wiadomosc);
                }
                if (string.IsNullOrEmpty(wiadomosc.Klient.Email) || string.IsNullOrEmpty(wiadomosc.Temat) || string.IsNullOrEmpty(wiadomosc.Tresc))
                {
                    ModelState.AddModelError("", "Uzupełnij e-mail, temat lub treść wiadomości.");
                    logger.Error("Brak e-maila, tematu lub treści wiadomości.");
                    return View(wiadomosc);
                }
                if (!Regex.IsMatch(wiadomosc.Klient.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    ModelState.AddModelError("E-mail", "Niepoprawny adres email.");
                    logger.Error($"Błędny adres e-mail: {wiadomosc.Klient.Email}");
                    return View(wiadomosc);
                }
                await emailService.WyslijEmailAsync(wiadomosc.Klient.Email, $"Witryna Firmowa, temat: {wiadomosc.Temat}", wiadomosc.Tresc);

                await dbService.DodajWiadomosc(wiadomosc);
                TempData["WyslanoEmail"] = "Wiadomość została wysłana.";
                logger.Info($"Wiadomość od: {wiadomosc.Klient.Email}, w temacie: {wiadomosc.Temat} została wysłana");
                return RedirectToAction("NapiszDoNas");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Wystąpił błąd podczas wysyłania wiadomości.");
                logger.Error($"Błąd podczas wysyłania wiadomości e-mail: {ex.Message}.");
                return RedirectToAction("");
            }
        }
    }
}
