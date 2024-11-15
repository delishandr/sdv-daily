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
            List<Villager> villagers = await db.Villagers.OrderBy(v => v.Name).ToListAsync();

            ViewBag.Title = "Villager List";
            ViewBag.ImageFolder = imageFolder;

            return View(villagers);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Villager? villager = await db.Villagers.FindAsync(id);
            if (villager == null)
            {
                return NotFound();
            }

            ViewBag.Title = "Edit Villager";
            ViewBag.Seasons = db.Seasons.ToList();

            return View(villager);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, BirthMonth, BirthDay, LovedGifts, CreatedAt")] Villager villager)
        {
            if (id != villager.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    villager.UpdatedAt = DateTime.Now;
                    db.Update(villager);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (db.Events.Find(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(villager);
        }
    }
}
