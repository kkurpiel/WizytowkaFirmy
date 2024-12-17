using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using WizytowkaFirmy.Models.DbModels;
using WizytowkaFirmy.Models;
using WizytowkaFirmy.Services;
using NLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace WizytowkaFirmy.Controllers
{
    public class OpinieKlientowController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private RecaptchaService recaptchaService;
        private DbService dbService;
        private JwtService jwtService = new JwtService();
        public OpinieKlientowController(IConfiguration configuration, DbService dbService, RecaptchaService recaptchaService)
        {
            this.configuration = configuration;
            this.recaptchaService = recaptchaService;
            this.dbService = dbService;
        }

        /// <summary>
        /// Wyświetl formularz pozwalający na dodanie nowej opinii i listę opinii wystawionych przez klientów
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/opinie-klientow")]
        public async Task<IActionResult> OpinieKlientow()
        {
            try
            {
                var model = new OpinieKlientowModel
                {
                    ListaOpinii = await dbService.OpinieKlientow(),
                    NowaOpinia = new OpiniaKlienta()
                };
                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error($"Błąd podczas ładowania opinii klientów: {ex.Message}");
                return RedirectToAction("");
            }
        }

        /// <summary>
        /// Dodaj nową opinię
        /// TODO: Stwórz możliwość usuwania wybranych opinii przez administratora
        /// </summary>
        /// <param name="model"></param>
        /// <param name="RecaptchaResponse"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/opinie-klientow")]
        public async Task<IActionResult> OpinieKlientow(OpinieKlientowModel model, string RecaptchaResponse, string? token, int id)
        {
            try
            {
                var config = configuration.GetSection("reCAPTCHA");
                if (!await recaptchaService.WeryfikacjareCAPTCHA(RecaptchaResponse, config["SecretKey"]))
                {
                    ModelState.AddModelError("", "Weryfikacja reCAPTCHA zakończyła się niepowodzeniem.");
                    logger.Error("Weryfikacja reCAPTCHA zakończyła się niepowodzeniem.");

                    model.ListaOpinii = await dbService.OpinieKlientow();
                    return View(model);
                }
                if (string.IsNullOrEmpty(model.NowaOpinia.Klient.Email))
                {
                    ModelState.AddModelError("", "Uzupełnij e-mail, temat.");
                    logger.Error("Brak adresu e-mail.");
                    model.ListaOpinii = await dbService.OpinieKlientow();
                    return View(model);
                }
                if (!Regex.IsMatch(model.NowaOpinia.Klient.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    ModelState.AddModelError("Email", "Niepoprawny adres email.");
                    logger.Error($"Błędny adres e-mail: {model.NowaOpinia.Klient.Email}");
                    model.ListaOpinii = await dbService.OpinieKlientow();
                    return View(model);
                }
                await dbService.DodajOpinie(model.NowaOpinia);
                TempData["WystawionoOpinie"] = "Opinia została wystawiona.";
                logger.Info($"Opinia od: {model.NowaOpinia.Klient.Email}: {model.NowaOpinia.Ocena}/5 została wystawiona");
                model.ListaOpinii = await dbService.OpinieKlientow();
                return RedirectToAction("OpinieKlientow");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Wystąpił błąd podczas wystawiania opinii.");
                logger.Error($"Błąd podczas dodawania nowej opinii: {ex.Message}");
                return RedirectToAction("");
            }
        }
        [HttpGet]
        [Route("/opinie-klientow/admin")]
        public async Task<IActionResult> OpinieKlientowAdmin(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token) || !jwtService.WalidacjaTokenuJwt(token))
                {
                    return Unauthorized("Nieprawidłowy lub brakujący token JWT.");
                }
                ViewBag.Token = jwtService.GenerujTokenJwt("admin", 15);
                var model = new OpinieKlientowModel
                {
                    ListaOpinii = await dbService.OpinieKlientow(),
                    NowaOpinia = new OpiniaKlienta()
                };
                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error($"Błąd podczas ładowania opinii klientów ADMIN: {ex.Message}");
                return RedirectToAction("");
            }
        }
        [HttpPost]
        [Route("/opinie-klientow/admin")]
        public async Task<IActionResult> OpinieKlientowAdmin(int id, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token) || !jwtService.WalidacjaTokenuJwt(token))
                {
                    return Unauthorized("Nieprawidłowy token.");
                }
                await dbService.UkryjOpinie(id); 
                TempData["UkrytoOpinie"] = "Opinia została ukryta.";
                return RedirectToAction("OpinieKlientowAdmin", new { token });
            }
            catch (Exception ex)
            {
                logger.Error($"Błąd podczas ukrywania opinii: {ex.Message}");
                return RedirectToAction("OpinieKlientowAdmin", new { token });
            }
        }
    }
}
