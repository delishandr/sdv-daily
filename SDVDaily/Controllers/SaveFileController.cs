using Microsoft.AspNetCore.Mvc;
using SDVDaily.Models;

namespace SDVDaily.Controllers
{
    public class SaveFileController : Controller
    {
        private DB_SDV_DailyContext db;

        public SaveFileController(DB_SDV_DailyContext _db)
        {
            db = _db;
        }

        public IActionResult New()
        {
            ViewBag.Title = "Add New Farm";
            ViewBag.Seasons = db.Seasons.ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> New(SaveFile saveFile)
        {


            return RedirectToAction("Index", "Home");
        }
    }
}
