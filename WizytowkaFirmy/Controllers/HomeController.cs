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
        /// Statyczny widok strony g��wnej projektu, wy�wietla podstawowe dane na temat firmy i jej w�a�ciciela
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
                logger.Error($"B��d podczas �adowania strony g��wnej{ex.Message}");
                return View();
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Statyczny widok strony wy�wietlaj�cej map� z lokalizacj� firmy i jej kr�tk� histori�
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
                logger.Error($"B��d podczas �adowania informacji o firmie: {ex.Message}.");
                return RedirectToAction("");
            }
        }
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Informacje na temat prywatno�ci.
        /// TODO: Informacje powinny wy�wietla� si� w stopce podczas wej�cia na stron�
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Widok wy�wietlany podczas wyst�pienia b��du
        /// TODO: Zmieni� wygl�d widoku.
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
