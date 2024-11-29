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
            // Method-based syntax
            var query = db.Villagers.Where(v => !v.IsDeleted).OrderBy(v => v.Name);

            // Query syntax
            var query1 = from v in db.Villagers where !v.IsDeleted orderby v.Name select v;

            // Raw SQL syntax
            var query2 = db.Villagers
                .FromSqlRaw("select * from villager where isDeleted = 0 order by name");

            List<Villager> villagers = await query.ToListAsync();

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
                    DateTime updateTime = DateTime.Now;

                    // Raw SQL syntax
                    //var rows = db.Database
                    //    .ExecuteSqlInterpolated(
                    //        $"update villager set name = {villager.Name}, birthMonth = {villager.BirthMonth}, birthDay = {villager.BirthDay}, lovedGifts = {villager.LovedGifts}, updatedAt = {updateTime.ToString()} where id = {id}");

                    // Method-based syntax
                    villager.UpdatedAt = updateTime;
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
                return RedirectToAction("Index");
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
                DateTime updateTime = DateTime.Now;

                // Raw SQL method
                //var rows = db.Database
                //    .ExecuteSqlInterpolated(
                //        $"update villager set isDeleted = 1, updatedAt = {updateTime.ToString()} where id = {villager.Id}");

                // Method-based syntax
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
