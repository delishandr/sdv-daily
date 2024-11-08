using Microsoft.AspNetCore.Mvc;
using SDVDaily.Models;
using System.Diagnostics;

namespace SDVDaily.Controllers
{
    public class HomeController : Controller
    {
        private DB_SDV_DailyContext db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DB_SDV_DailyContext _db)
        {
            db = _db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("userId").HasValue)
            {
                ViewBag.Year = HttpContext.Session.GetInt32("saveYear");
                ViewBag.Season = db.Seasons.Where(s => s.Id == HttpContext.Session.GetInt32("saveSeason")).Select(s => s.Name).First();
                ViewBag.Day = (int)HttpContext.Session.GetInt32("saveDay")!;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
