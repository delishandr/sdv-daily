using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDVDaily.Models;
using System.Net;

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
            List<Villager> villagers = await db.Villagers.Where(v => !v.IsDeleted).OrderBy(v => v.Name).ToListAsync();

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

                    HttpContext.Session.SetString("infoMsg", "Villager updated!");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (db.Events.Find(id) == null)
                    {
                        HttpContext.Session.SetString("errMsg", "Error: ID not found!");
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

        [HttpGet]
        public IActionResult Delete(int id)
        {
            ViewBag.Id = id;
            ViewBag.Title = "Delete Villager";

            return View();
        }

        [HttpPost]
        public async Task<ResponseViewModel<Villager>> Delete(Villager villager)
        {
            ResponseViewModel<Villager> response = new ResponseViewModel<Villager>();

            Villager? extVillager = db.Villagers.Find(villager.Id);
            if (extVillager == null)
            {
                response.statusCode = HttpStatusCode.BadRequest;
                response.message = "ID not found!";
            }
            else
            {
                extVillager.IsDeleted = true;
                extVillager.UpdatedAt = DateTime.Now;
                db.Update(extVillager);

                await db.SaveChangesAsync();

                response.statusCode = HttpStatusCode.OK;
                response.message = "Villager deleted!";
            }

            return response;
        }
    }
}
