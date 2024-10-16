using Microsoft.AspNetCore.Mvc;
using SDVDaily.Models;

namespace SDVDaily.Controllers
{
    public class VillagerController : Controller
    {
        private DB_SDV_DailyContext db;

        public VillagerController(DB_SDV_DailyContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            return View(db.Villagers.ToList());
        }
    }
}
