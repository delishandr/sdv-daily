using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDVDaily.Models;
using System.Net;

namespace SDVDaily.Controllers
{
    public class GrowingCropController : Controller
    {
        private DB_SDV_DailyContext db;
        private readonly string imageFolder;

        public GrowingCropController(IConfiguration _config, DB_SDV_DailyContext _db)
        {
            db = _db;
            imageFolder = _config["ImageFolder"];
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (!HttpContext.Session.GetInt32("saveId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.IsAgriculturist = db.SaveFiles
                .Where(s => s.Id == HttpContext.Session.GetInt32("saveId")).Single().IsAgriculturist;
            ViewBag.Title = "Add Growing Crop";

            return View();
        }

        [HttpPost]
        public async Task<ResponseViewModel<GrowingCrop>> Add(GrowingCropViewModel addCrop)
        {
            ResponseViewModel<GrowingCrop> response = new ResponseViewModel<GrowingCrop>();
            Crop? extCrop = db.Crops.Where(c => c.Id == addCrop.CropId).FirstOrDefault();
            if (extCrop == null)
            {
                return response;
            }

            if (!HttpContext.Session.GetInt32("saveId").HasValue)
            {
                return response;
            }

            SaveFile file = db.SaveFiles.Where(s => s.Id == HttpContext.Session.GetInt32("saveId")).First();

            GrowingCrop crop = new GrowingCrop();
            crop.SaveId = file.Id;
            crop.CropId = extCrop.Id;
            crop.Amount = addCrop.Amount;
            crop.IsOnGinger = addCrop.IsOnGinger;
            crop.IsIndoors = addCrop.IsIndoors;

            double percent = 100;
            if (addCrop.IsAgriculturist)
                percent -= 10;

            if (addCrop.IsSG)
                percent -= 10;
            else if (addCrop.IsDSG)
                percent -= 25;
            else if (addCrop.IsHSG)
                percent -= 33;

            int growth = (int)Math.Floor(extCrop.GrowthTime * percent / 100);

            CropSeason? cs = new CropSeason();

            int harvest = file.Day + growth;
            if (harvest > 28)
            {
                if (!addCrop.IsIndoors && !addCrop.IsOnGinger)
                    cs = db.CropSeasons.Where(cs => cs.CropId == extCrop.Id && cs.SeasonId == file.Season + 1).FirstOrDefault();

                crop.NextHarvestSeason = file.Season + 1;
                crop.NextHarvest = harvest - 28;

            }
            else
            {
                crop.NextHarvest = harvest;
                crop.NextHarvestSeason = file.Season;
            }

            GrowingCrop? extGrown = db.GrowingCrops
                .Where(g => g.SaveId == file.Id && g.CropId == extCrop.Id
                    && g.NextHarvest == crop.NextHarvest && g.NextHarvestSeason == crop.NextHarvestSeason
                    && g.IsOnGinger == crop.IsOnGinger && g.IsIndoors == crop.IsIndoors)
                .FirstOrDefault();

            if (extGrown == null)
            {
                // ORM Syntax
                db.Add(crop);

                // Raw SQL syntax
                //db.Database
                //    .ExecuteSqlInterpolated(
                //        $"insert into growing_crop (saveId, cropId, nextHarvest, nextHarvestSeason, amount, isOnGinger, isIndoors) values ({crop.SaveId}, {crop.CropId}, {crop.NextHarvest}, {crop.NextHarvestSeason}, {crop.Amount}, {crop.IsOnGinger}, {crop.IsIndoors})"
                //    );
            }
            else
            {
                // ORM syntax
                extGrown.Amount += addCrop.Amount;
                extGrown.UpdatedAt = DateTime.Now;
                db.Update(extGrown);

                // Raw SQL syntax
                //db.Database
                //    .ExecuteSqlInterpolated(
                //        $"update growing_crop set amount = {extGrown.Amount}, updatedAt = {DateTime.Now.ToString()} where id = {extGrown.Id}"
                //    );
            }
            await db.SaveChangesAsync();

            response.data = crop;
            response.statusCode = cs != null ? HttpStatusCode.Created : HttpStatusCode.OK;
            response.message = cs != null ? "Crop added!" : "Warning: Crop might not be harvestable in time!";

            return response;
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!HttpContext.Session.GetInt32("saveId").HasValue)
            {
                return RedirectToAction("ManageFarm", "Home");
            }

            GrowingCrop? data = db.GrowingCrops.Find(id);
            GrowingCropViewModel crop = new GrowingCropViewModel();
            if (data == null)
            {
                return RedirectToAction("ManageFarm", "Home");
            }
            else
            {
                crop.Id = id;
                crop.CropId = data.CropId;
                crop.CropName = db.Crops.Where(c => c.Id == data.CropId).Single().Name;
                crop.Amount = data.Amount;
            }
            
            ViewBag.Title = "Delete Crop";

            return View(crop);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(GrowingCrop crop)
        {
            GrowingCrop? extCrop = db.GrowingCrops.Find(crop.Id);
            if (extCrop == null)
            {
                return NotFound();
            }
            else
            {
                if (crop.Amount == extCrop.Amount)
                {
                    // Raw SQL syntax
                    //db.Database.ExecuteSqlInterpolated(
                    //    $"delete from growing_crop where id = {crop.Id}"    
                    //);

                    // ORM syntax
                    db.Remove(extCrop);
                }
                else
                {
                    DateTime updatedAt = DateTime.Now;

                    // Raw SQL syntax
                    //db.Database.ExecuteSqlInterpolated(
                    //    $"update growing_crop set amount = {extCrop.Amount - crop.Amount}, updatedAt = {updatedAt.ToString()} where id = {crop.Id}"    
                    //);

                    // ORM syntax
                    extCrop.Amount -= crop.Amount;
                    extCrop.UpdatedAt = updatedAt;
                    db.Update(extCrop);
                }
                await db.SaveChangesAsync();
            }

            return RedirectToAction("ManageFarm", "Home");
        }
    }
}
