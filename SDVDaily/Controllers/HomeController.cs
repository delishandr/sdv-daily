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
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DB_SDV_DailyContext _db)
        {
            db = _db;
            _logger = logger;
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
                
                // TODO: delete withering crops on season change (day == 1)
                //      condition: !IsOnGinger && !IsIndoors

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
    }
}
