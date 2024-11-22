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
        public async Task<IActionResult> New([Bind("Name, Day, Season, Year, HasPet, HasFarmAnimals, IsAgriculturist")]  SaveFile saveFile)
        {
            if (HttpContext.Session.GetInt32("userId") != null)
            {
                saveFile.UserId = (int)HttpContext.Session.GetInt32("userId")!;
                db.Add(saveFile);

                await db.SaveChangesAsync();

                HttpContext.Session.SetInt32("saveId", saveFile.Id);
                HttpContext.Session.SetString("saveName", saveFile.Name);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
