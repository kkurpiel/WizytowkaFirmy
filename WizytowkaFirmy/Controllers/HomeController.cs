using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;
using System.Diagnostics;
using System.Text.RegularExpressions;
using WizytowkaFirmy.Models;
using WizytowkaFirmy.Models.DbModels;
using WizytowkaFirmy.Services;

namespace WizytowkaFirmy.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private DbService dbService;
        public HomeController(IConfiguration configuration, DbService dbService)
        {
            this.configuration = configuration;
            this.dbService = dbService;
        }
        
        /// <summary>
        /// Statyczny widok strony g³ównej projektu, wyœwietla podstawowe dane na temat firmy i jej w³aœciciela
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex) 
            {
                logger.Error($"B³¹d podczas ³adowania strony g³ównej{ex.Message}");
                return View();
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Statyczny widok strony wyœwietlaj¹cej mapê z lokalizacj¹ firmy i jej krótk¹ historiê
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/o-nas")]
        public IActionResult ONas()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                logger.Error($"B³¹d podczas ³adowania informacji o firmie: {ex.Message}.");
                return RedirectToAction("");
            }
        }
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Informacje na temat prywatnoœci.
        /// TODO: Informacje powinny wyœwietlaæ siê w stopce podczas wejœcia na stronê
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Widok wyœwietlany podczas wyst¹pienia b³êdu
        /// TODO: Zmieniæ wygl¹d widoku.
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
