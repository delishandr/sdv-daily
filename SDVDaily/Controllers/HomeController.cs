using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDVDaily.Models;
using System.Diagnostics;
using System.Net;

namespace SDVDaily.Controllers
{
    public class HomeController : Controller
    {
        private DB_SDV_DailyContext db;
        private readonly string imageFolder;

        public HomeController(IConfiguration _config, DB_SDV_DailyContext _db)
        {
            db = _db;
            imageFolder = _config["ImageFolder"];
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("saveId").HasValue)
            {
                SaveFile file = await db.SaveFiles.Where(s => s.Id == HttpContext.Session.GetInt32("saveId")).FirstAsync();

                ViewBag.Year = file.Year;
                ViewBag.Season = db.Seasons.Where(s => s.Id == file.Season).Select(s => s.Name).First();
                ViewBag.Day = file.Day;
                ViewBag.HasFarmAnimals = file.HasFarmAnimals;
                ViewBag.HasPet = file.HasPet;

                List<GrowingCropViewModel> vmHarvest = new List<GrowingCropViewModel>();

                List<GrowingCrop> harvests = db.GrowingCrops
                    .Where(g => g.NextHarvest == file.Day && g.NextHarvestSeason == file.Season)
                    .ToList();
                foreach (GrowingCrop harvest in harvests)
                {
                    Crop crop = db.Crops.Where(c => c.Id == harvest.CropId).Single();
                    GrowingCropViewModel h = new GrowingCropViewModel()
                    {
                        Id = harvest.Id,
                        CropId = crop.Id,
                        CropName = crop.Name,
                        Amount = harvest.Amount,
                        IsOnGinger = harvest.IsOnGinger,
                        IsIndoors = harvest.IsIndoors
                    };
                    vmHarvest.Add(h);

                }
                ViewBag.Harvest = vmHarvest.OrderBy(h => h.CropName).ToList();

                ViewBag.Birthday = db.Villagers
                    .Where(v => !v.IsDeleted && v.BirthMonth == file.Season && v.BirthDay == file.Day)
                    .FirstOrDefault();
                ViewBag.Event = (
                    from e in db.Events
                    join ed in db.EventDays
                        on e.Id equals ed.EventId
                    where !e.IsDeleted && ed.Day == file.Day && ed.Season == file.Season
                    select new Event { 
                        Id = e.Id,
                        Name = e.Name,
                        Type = e.Type,
                        Location = e.Location,
                        StartTime = e.StartTime,
                        EndTime = e.EndTime,
                        Preparation = e.Preparation
                    }
                ).FirstOrDefault();

                ViewBag.ImageFolder = imageFolder;
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
                
                if (save.Day == 1)
                {
                    List<GrowingCrop> growingCrops = db.GrowingCrops
                        .Where(g => g.SaveId == save.Id && !g.IsOnGinger && !g.IsIndoors)
                        .ToList();

                    foreach (GrowingCrop growing in growingCrops)
                    {
                        CropSeason? season = db.CropSeasons
                            .Where(cs => cs.CropId == growing.CropId && cs.SeasonId == save.Season).FirstOrDefault();

                        if (season == null)
                            db.Remove(growing);
                    }
                }

                await db.SaveChangesAsync();
			}
            
            return RedirectToAction("Index", "Home");
		}

        [HttpPost]
        public async Task<ResponseViewModel<List<HarvestCheck>>> NextDayHarvest(List<HarvestCheck> harvestChecks)
        {
            ResponseViewModel<List<HarvestCheck>> response = new ResponseViewModel<List<HarvestCheck>>();
            if (HttpContext.Session.GetInt32("saveId").HasValue)
            {
                SaveFile save = db.SaveFiles.Where(s => s.Id == HttpContext.Session.GetInt32("saveId")).Single();

                foreach (HarvestCheck check in harvestChecks)
                {
                    GrowingCrop growingCrop = db.GrowingCrops.Where(g => g.Id == check.GrowingCropId).Single();

                    if (check.IsHarvested)
                    {
                        Crop crop = db.Crops.Where(c => c.Id == check.CropId).Single();
                        if (crop.RegrowthTime != null)
                        {
                            growingCrop.UpdatedAt = DateTime.Now;
                            growingCrop.NextHarvest += (int)crop.RegrowthTime;
                            if (growingCrop.NextHarvest > 28)
                            {
                                growingCrop.NextHarvest -= 28;
                                if (growingCrop.NextHarvestSeason == 4)
                                    growingCrop.NextHarvestSeason = 1;
                                else
                                    growingCrop.NextHarvestSeason++;
                            }
                            db.Update(growingCrop);
                        }
                        else
                        {
                            db.Remove(growingCrop);
                        }
                    }
                    else
                    {
                        if (growingCrop.NextHarvest == 28)
                        {
                            growingCrop.NextHarvest = 1;
                            if (growingCrop.NextHarvestSeason == 4)
                                growingCrop.NextHarvestSeason = 1;
                            else
                                growingCrop.NextHarvestSeason++;
                        }
                        else
                            growingCrop.NextHarvest++;
                        growingCrop.UpdatedAt = DateTime.Now;

                        db.Update(growingCrop); 
                    }
                }
                await db.SaveChangesAsync();

                response.data = harvestChecks;
                response.statusCode = HttpStatusCode.OK;
                response.message = "Crop successfully harvested!";
            }

            return response;
        }

        public async Task<IActionResult> ManageFarm()
        {
            if (HttpContext.Session.GetInt32("saveId") == null)
                return RedirectToAction("Index");

            SaveFile save = db.SaveFiles.Where(s => s.Id == HttpContext.Session.GetInt32("saveId")).First();

            List<GrowingCropViewModel> cropList = await (
                from g in db.GrowingCrops
                join c in db.Crops
                    on g.CropId equals c.Id
                join s in db.Seasons
                    on g.NextHarvestSeason equals s.Id
                where g.SaveId == HttpContext.Session.GetInt32("saveId")
                select new GrowingCropViewModel
                {
                    Id = g.Id,
                    CropId = g.CropId,
                    CropName = c.Name,
                    NextHarvest = g.NextHarvest,
                    NextHarvestSeason = g.NextHarvestSeason,
                    NextHarvestSeasonName = s.Name,
                    Amount = g.Amount,
                    IsOnGinger = g.IsOnGinger,
                    IsIndoors = g.IsIndoors
                }
            ).OrderBy(g => g.NextHarvestSeason).ThenBy(g => g.NextHarvest).ToListAsync();

            ViewBag.Title = "Manage Farm";
            ViewBag.ImageFolder = imageFolder;

            ViewBag.SaveId = save.Id;
            ViewBag.HasPet = save.HasPet;
            ViewBag.HasFarmAnimals = save.HasFarmAnimals;
            ViewBag.IsAgriculturist = save.IsAgriculturist;

            return View(cropList);
        }

        public async Task<IActionResult> UpdateFarm([Bind("Id, HasPet, HasFarmAnimals, IsAgriculturist")] SaveFile file)
        {
            SaveFile? extFile = await db.SaveFiles.FindAsync(file.Id);

            if (extFile == null)
            {
                return NotFound();
            }

            extFile.HasPet = file.HasPet;
            extFile.HasFarmAnimals = file.HasFarmAnimals;
            extFile.IsAgriculturist = file.IsAgriculturist;
            extFile.UpdatedAt = DateTime.Now;
            db.Update(extFile);
            await db.SaveChangesAsync();

            HttpContext.Session.SetString("infoMsg", "Farm updated!");

            return RedirectToAction("ManageFarm");
        }
    }
}
