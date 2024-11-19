using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDVDaily.Models;
using System.Net;

namespace SDVDaily.Controllers
{
    public class CropController : Controller
    {
        private DB_SDV_DailyContext db;
        private readonly string imageFolder;
        private readonly int pageSize;

        public CropController(IConfiguration _config, DB_SDV_DailyContext _db)
        {
            db = _db;
            imageFolder = _config["ImageFolder"];
            pageSize = int.Parse(_config["PageSize"]);
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var cropList = await db.Crops.ToListAsync();
            List<CropViewModel> items = new List<CropViewModel>();

            foreach (Crop crop in cropList)
            {
                CropViewModel item = new CropViewModel();
                item.Id = crop.Id;
                item.Name = crop.Name;

                item.CategoryId = crop.CategoryId;
                var category = db.CropCategories.Where(cc => cc.Id.Equals(crop.CategoryId)).FirstOrDefault();
                item.CategoryName = (category != null ? category.Name : "");

                item.GrowthTime = crop.GrowthTime;
                item.RegrowthTime = crop.RegrowthTime;
                item.Unirrigated = crop.Unirrigated;
                item.IsWalkable = crop.IsWalkable;
                item.StartYear = crop.StartYear;
                item.SellPrice = crop.SellPrice;
                item.Img = crop.Img;

                var cropSeasonList = db.CropSeasons.Where(c => c.CropId.Equals(crop.Id)).ToList();
                List<int> seasonIds = cropSeasonList.Select(c => c.SeasonId).ToList();
                item.SeasonIds = seasonIds.ToArray();

                foreach (CropSeason cropSeason in cropSeasonList)
                {
                    item.Seasons.Add(db.Seasons.Where(s => s.Id.Equals(cropSeason.SeasonId)).Single<Season>());
                }

                item.CreatedAt = crop.CreatedAt;
                item.UpdatedAt = crop.UpdatedAt;
                item.IsDeleted = crop.IsDeleted;

                items.Add(item);
            }

            int startIdx = (page - 1) * 10;

            ViewBag.TotalItems = items.Count;
            ViewBag.Page = page;
            ViewBag.StartItem = startIdx + 1;
            ViewBag.PageSize = Math.Min(pageSize, items.Count - (page - 1) * pageSize);
            ViewBag.TotalPages = Math.Ceiling(items.Count / (double)pageSize);

            items = items.GetRange(startIdx, ViewBag.PageSize);

            ViewBag.Title = "Crop List";
            ViewBag.ImageFolder = imageFolder;

			return View(items);
        }

        public async Task<IActionResult> Detail(int id)
            {
            Crop? findCrop = await db.Crops.FindAsync(id);
            if (findCrop == null)
            {
                return NotFound();
            }

            CropViewModel crop = new CropViewModel();
            crop.Id = findCrop.Id;
            crop.Name = findCrop.Name;

            crop.CategoryId = findCrop.CategoryId;
            var category = db.CropCategories.Where(cc => cc.Id.Equals(findCrop.CategoryId)).FirstOrDefault();
            crop.CategoryName = (category != null ? category.Name : "");

            crop.GrowthTime = findCrop.GrowthTime;
            crop.RegrowthTime = findCrop.RegrowthTime;
            crop.Unirrigated = findCrop.Unirrigated;
            crop.IsWalkable = findCrop.IsWalkable;
            crop.StartYear = findCrop.StartYear;
            crop.SellPrice = findCrop.SellPrice;
            crop.Img = findCrop.Img;

            var cropSeasonList = db.CropSeasons.Where(c => c.CropId.Equals(findCrop.Id)).ToList();
            List<int> seasonIds = cropSeasonList.Select(c => c.SeasonId).ToList();
            crop.SeasonIds = seasonIds.ToArray();

            foreach (CropSeason cropSeason in cropSeasonList)
            {
                crop.Seasons.Add(db.Seasons.Where(s => s.Id.Equals(cropSeason.SeasonId)).Single<Season>());
            }

            crop.CreatedAt = findCrop.CreatedAt;
            crop.UpdatedAt = findCrop.UpdatedAt;
            crop.IsDeleted = findCrop.IsDeleted;

            ViewBag.Title = "Crop Detail";
            ViewBag.ImageFolder = imageFolder;

            return View(crop);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Crop? findCrop = await db.Crops.FindAsync(id);
            if (findCrop == null)
            {
                return NotFound();
            }

            CropViewModel crop = new CropViewModel();
            crop.Id = findCrop.Id;
            crop.Name = findCrop.Name;

            crop.CategoryId = findCrop.CategoryId;
            var category = db.CropCategories.Where(cc => cc.Id.Equals(findCrop.CategoryId)).FirstOrDefault();
            crop.CategoryName = (category != null ? category.Name : "");

            crop.GrowthTime = findCrop.GrowthTime;
            crop.RegrowthTime = findCrop.RegrowthTime;
            crop.Unirrigated = findCrop.Unirrigated;
            crop.IsWalkable = findCrop.IsWalkable;
            crop.StartYear = findCrop.StartYear;
            crop.SellPrice = findCrop.SellPrice;
            crop.Img = findCrop.Img;

            var cropSeasonList = db.CropSeasons.Where(c => c.CropId.Equals(findCrop.Id)).ToList();
            List<int> seasonIds = cropSeasonList.Select(c => c.SeasonId).ToList();
            crop.SeasonIds = seasonIds.ToArray();

            foreach (CropSeason cropSeason in cropSeasonList)
            {
                crop.Seasons.Add(db.Seasons.Where(s => s.Id.Equals(cropSeason.SeasonId)).Single<Season>());
            }

            crop.CreatedAt = findCrop.CreatedAt;
            crop.UpdatedAt = findCrop.UpdatedAt;
            crop.IsDeleted = findCrop.IsDeleted;

            ViewBag.Title = "Edit Crop";
            ViewBag.ImageFolder = imageFolder;

            return View(crop);
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
            ViewBag.Title = "Add Crop";

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
                db.Add(crop);
            }
            else
            {
                extGrown.Amount += addCrop.Amount;
                extGrown.UpdatedAt = DateTime.Now;
                db.Update(extGrown);
            }
            await db.SaveChangesAsync();

            response.data = crop;
            response.statusCode = cs != null ? HttpStatusCode.Created : HttpStatusCode.OK;
            response.message = cs != null ? "Crop added!" : "Warning: Crop might not be harvewtable in time!";

            return response;
        }

        public async Task<ResponseViewModel<List<CropViewModel>>> GetCropsBy(string category)
        {
            ResponseViewModel<List<CropViewModel>> response = new ResponseViewModel<List<CropViewModel>>();

            if (!HttpContext.Session.GetInt32("saveId").HasValue)
            {
                return response;
            }

            SaveFile file = db.SaveFiles.Where(s => s.Id == HttpContext.Session.GetInt32("saveId")).First();
            int? year = file.Year;
            int? curSeason = file.Season;

            List<Crop> crops = await db.Crops.Where(c => !c.IsDeleted && c.StartYear <= year).ToListAsync();

            List<CropViewModel> items = new List<CropViewModel>();

            foreach (Crop crop in crops)
            {
                var cropSeasonList = db.CropSeasons.Where(c => c.CropId.Equals(crop.Id)).ToList();
                CropViewModel item = new CropViewModel();

                switch (category)
                {
                    case "season":
                        if (cropSeasonList.Any(cs => cs.SeasonId == curSeason))
                        {
                            item.Id = crop.Id;
                            item.Name = crop.Name;
                            item.GrowthTime = crop.GrowthTime;

                            foreach (CropSeason cropSeason in cropSeasonList)
                            {
                                item.Seasons.Add(db.Seasons.Where(s => s.Id.Equals(cropSeason.SeasonId)).Single<Season>());
                            }

                            items.Add(item);
                        }
                        break;
                    case "ginger":
                        if (cropSeasonList.Any(cs => cs.SeasonId == 2)) // season: summer
                        {
                            item.Id = crop.Id;
                            item.Name = crop.Name;
                            item.GrowthTime = crop.GrowthTime;

                            foreach (CropSeason cropSeason in cropSeasonList)
                            {
                                item.Seasons.Add(db.Seasons.Where(s => s.Id.Equals(cropSeason.SeasonId)).Single<Season>());
                            }

                            items.Add(item);
                        }
                        break;
                    case "all":
                        item.Id = crop.Id;
                        item.Name = crop.Name;
                        item.GrowthTime = crop.GrowthTime;

                        foreach (CropSeason cropSeason in cropSeasonList)
                        {
                            item.Seasons.Add(db.Seasons.Where(s => s.Id.Equals(cropSeason.SeasonId)).Single<Season>());
                        }

                        items.Add(item);

                        break;
                    default:
                        break;
                }
            }
            items = items.OrderBy(c => c.Name).ToList();

            response.data = items;
            response.statusCode = items.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NoContent;
            response.message = items.Count > 0 ? $"{response.statusCode} - {items.Count} data found" : $"{response.statusCode} - No data found";

            return response;
        }
    }
}
