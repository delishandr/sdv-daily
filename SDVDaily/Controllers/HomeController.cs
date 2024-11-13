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
            if (HttpContext.Session.GetInt32("saveId").HasValue)
            {
                SaveFile file = db.SaveFiles.Where(s => s.Id == HttpContext.Session.GetInt32("saveId")).First();

                ViewBag.Year = file.Year;
                ViewBag.Season = db.Seasons.Where(s => s.Id == file.Season).Select(s => s.Name).First();
                ViewBag.Day = file.Day;
                ViewBag.HasFarmAnimals = file.HasFarmAnimals;
                ViewBag.HasPet = file.HasPet;

                List<Crop> crops = new List<Crop>();
                List<GrowingCrop> harvests = db.GrowingCrops
                    .Where(g => g.NextHarvest == file.Day && g.NextHarvestSeason == file.Season)
                    .ToList();
                foreach (GrowingCrop harvest in harvests)
                {
                    Crop crop = db.Crops.Where(c => c.Id == harvest.CropId).Single();
                    crops.Add(crop);
                }
                ViewBag.Harvest = crops.OrderBy(c => c.Name).ToList();
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

        public async Task<IActionResult> NextDay()
        {
			if (HttpContext.Session.GetInt32("saveId").HasValue)
            {
                SaveFile save = db.SaveFiles.Where(s => s.Id == HttpContext.Session.GetInt32("saveId")).First();

                if (save.Day == 28)
                {
                    if (save.Season == 4)
                    {
                        save.Year++;
                        save.Season = 1;
                    }
                    else
                        save.Season++;
                    save.Day = 1;
                }
                else
                {
                    save.Day++;
                }
                save.UpdatedAt = DateTime.Now;
                db.Update(save);
                await db.SaveChangesAsync();
			}
            
            return RedirectToAction("Index", "Home");
		}
    }
}
