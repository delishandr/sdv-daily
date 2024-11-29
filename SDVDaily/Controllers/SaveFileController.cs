using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                if (saveFile.Day == 0)
                {
                    saveFile.Day = 1;
                    saveFile.Season = 1;
                    saveFile.Year = 1;
                }
                saveFile.UserId = (int)HttpContext.Session.GetInt32("userId")!;

                // ORM syntax
                db.Add(saveFile);
                await db.SaveChangesAsync();

                // Raw SQL syntax
                //db.Database.ExecuteSqlInterpolated(
                //    $"insert into save_file (name, day, season, year, hasPet, hasFarmAnimals, isAgriculturist) values ({saveFile.Name}, {saveFile.Day}, {saveFile.Season}, {saveFile.Year}, {saveFile.HasPet}, {saveFile.HasFarmAnimals}, {saveFile.IsAgriculturist})"    
                //);

                return await Change(saveFile.Id);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Change()
        {
            List<SaveFile> saves = await db.SaveFiles.Where(s => s.UserId == HttpContext.Session.GetInt32("userId")).ToListAsync();
            List<SaveFileViewModel> vmSaves = new List<SaveFileViewModel>();

            foreach (SaveFile save in saves)
            {
                SaveFileViewModel vmSave = new SaveFileViewModel()
                {
                    Id = save.Id,
                    Name = save.Name,
                    Day = save.Day,
                    Season = save.Season,
                    SeasonName = db.Seasons.Where(s => s.Id == save.Season).Single().Name,
                    Year = save.Year
                };
                vmSaves.Add(vmSave);
            }

            ViewBag.Title = "Change Farm";

            return View(vmSaves);
        }

        [HttpPost]
        public async Task<IActionResult> Change(int SaveId)
        {
            SaveFile? file = db.SaveFiles.Find(SaveId);
            if (file == null)
            {
                return NotFound();
            }

            // ORM Method
            User user = db.Users.Where(u => u.Id == HttpContext.Session.GetInt32("userId")).Single();
            user.LastSave = SaveId;
            db.Update(user);

            // Raw SQL method
            //db.Database.ExecuteSqlInterpolated(
            //    $"update user set lastSave = {SaveId} where id = {HttpContext.Session.GetInt32("userId")}"    
            //);

            await db.SaveChangesAsync();

            HttpContext.Session.SetInt32("saveId", SaveId);
			HttpContext.Session.SetString("saveName", file.Name);

            HttpContext.Session.SetString("infoMsg", "Farm changed successfully!");

			return RedirectToAction("Index", "Home");
        }
    }
}
