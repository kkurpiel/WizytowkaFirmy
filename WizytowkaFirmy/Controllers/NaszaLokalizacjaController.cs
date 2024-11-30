using Microsoft.AspNetCore.Mvc;
using NLog;

namespace WizytowkaFirmy.Controllers
{
    public class NaszaLokalizacjaController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public NaszaLokalizacjaController(ILogger<HomeController> _logger)
        {
            this._logger = _logger;
        }

        [HttpGet]
        [Route("/NaszaLokalizacja")]
        public IActionResult NaszaLokalizacja()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            { 
                logger.Error(ex.Message);
                return RedirectToAction("Index");
            }
        }
    }
}
