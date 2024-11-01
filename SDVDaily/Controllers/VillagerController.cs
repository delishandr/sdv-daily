using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDVDaily.Models;

namespace SDVDaily.Controllers
{
    public class VillagerController : Controller
    {
        private DB_SDV_DailyContext db;
        private readonly string imageFolder;

        public VillagerController(IConfiguration _config, DB_SDV_DailyContext _db)
        {
            db = _db;
            imageFolder = _config["ImageFolder"];
        }
        
        public async Task<IActionResult> Index()
        {
            List<Villager> villagers = await db.Villagers.ToListAsync();

            ViewBag.Title = "Villager List";
            ViewBag.ImageFolder = imageFolder;

            return View(villagers);
        }
    }
}
