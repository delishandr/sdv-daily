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
            // Method-based syntax
            var query = db.Crops.Where(c => !c.IsDeleted);

            // Query syntax
            var query1 = from c in db.Crops where !c.IsDeleted select c;

            // Raw SQL syntax
            var query2 = db.Crops
                .FromSqlRaw("select * from crop where isDeleted = 0");

            List<Crop> cropList = await query.ToListAsync();

            List<CropViewModel> items = new List<CropViewModel>();

            foreach (Crop crop in cropList)
            {
                CropViewModel item = new CropViewModel();
                item.Id = crop.Id;
                item.Name = crop.Name;

                item.CategoryId = crop.CategoryId;
                var category = db.CropCategories
                    .Where(cc => cc.Id.Equals(crop.CategoryId)).FirstOrDefault();
                item.CategoryName = (category != null ? category.Name : "");

                item.GrowthTime = crop.GrowthTime;
                item.RegrowthTime = crop.RegrowthTime;
                item.Unirrigated = crop.Unirrigated;
                item.IsWalkable = crop.IsWalkable;
                item.StartYear = crop.StartYear;
                item.SellPrice = crop.SellPrice;
                item.Img = crop.Img;

                var cropSeasonList = db.CropSeasons
                    .Where(c => c.CropId.Equals(crop.Id)).ToList();
                List<int> seasonIds = cropSeasonList.Select(c => c.SeasonId).ToList();
                item.SeasonIds = seasonIds.ToArray();

                foreach (CropSeason cropSeason in cropSeasonList)
                {
                    item.Seasons.Add(db.Seasons
                        .Where(s => s.Id.Equals(cropSeason.SeasonId)).Single<Season>());
                }

                item.CreatedAt = crop.CreatedAt;
                item.UpdatedAt = crop.UpdatedAt;
                item.IsDeleted = crop.IsDeleted;

                items.Add(item);
            }

            int startIdx = (page - 1) * pageSize;

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

            // Method-based syntax
            var query = db.Crops
                .Where(c => !c.IsDeleted && c.Id == id);

            // Query syntax
            var query1 = from c in db.Crops where !c.IsDeleted && c.Id == id select c;

            // Raw SQL syntax
            var query2 = db.Crops
                .FromSqlRaw($"select * from crop where isDeleted = 0 and id = {id}");

            Crop? findCrop = await query.FirstOrDefaultAsync();

            if (findCrop == null)
            {
                return NotFound();
            }

            CropViewModel crop = new CropViewModel();
            crop.Id = findCrop.Id;
            crop.Name = findCrop.Name;
            crop.CategoryId = findCrop.CategoryId;
            var category = db.CropCategories
                .Where(cc => cc.Id.Equals(findCrop.CategoryId))
                .FirstOrDefault();
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

            crop.IsRegrowing = findCrop.RegrowthTime > 0;
            crop.IsUnirrigated = findCrop.Unirrigated > 0;

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

            ViewBag.Seasons = db.Seasons.ToList();
            ViewBag.Categories = db.CropCategories.ToList();

            ViewBag.Title = "Edit Crop";
            ViewBag.ImageFolder = imageFolder;

            return View(crop);
        }

        [HttpPost]
        public async Task<ResponseViewModel<Crop>> Edit(CropViewModel crop, string[] seasons)
        {
            ResponseViewModel<Crop> response = new ResponseViewModel<Crop>();

            Crop? extCrop = await db.Crops.FindAsync(crop.Id);
            if (extCrop == null)
            {
                response.statusCode = HttpStatusCode.BadRequest;
                response.message = "Crop not found!";
            }
            else
            {
                extCrop.Name = crop.Name;
                extCrop.CategoryId = crop.CategoryId;
                extCrop.GrowthTime = crop.GrowthTime;

                extCrop.RegrowthTime = crop.IsRegrowing ? crop.RegrowthTime : null;
                extCrop.Unirrigated = crop.IsUnirrigated ? crop.Unirrigated : null;

                extCrop.IsWalkable = crop.IsWalkable;
                extCrop.StartYear = crop.StartYear;
                extCrop.SellPrice = crop.SellPrice;
                extCrop.UpdatedAt = DateTime.Now;
                db.Update(extCrop);

                List<int> cropSeasons = await db.CropSeasons.Where(cs => cs.CropId == crop.Id).Select(cs => cs.SeasonId).ToListAsync();
                List<int> newSeasons = new List<int>();
                foreach (string s in seasons)
                    newSeasons.Add(int.Parse(s));

                // Removing seasons that are not in updated list
                List<int> notInNew = cropSeasons.Except(newSeasons).ToList();
                foreach (int season in notInNew)
                {
                    CropSeason removeCS = db.CropSeasons.Where(cs => cs.CropId == crop.Id && cs.SeasonId == season).Single();
                    db.Remove(removeCS);
                }

                // Adding seasons that are not in existing list
                List<int> notInExt = newSeasons.Except(cropSeasons).ToList();
                foreach (int season in notInExt)
                {
                    CropSeason addCS = new CropSeason()
                    {
                        CropId = crop.Id,
                        SeasonId = season
                    };
                    db.Add(addCS);
                }
                await db.SaveChangesAsync();

                response.statusCode = HttpStatusCode.OK;
                response.message = "Crop updated!";
                response.data = extCrop;
            }

            return response;
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            ViewBag.Id = id;
            ViewBag.Title = "Delete Crop";

            return View();
        }

        [HttpPost]
        public async Task<ResponseViewModel<Crop>> Delete(Crop crop)
        {
            ResponseViewModel<Crop> response = new ResponseViewModel<Crop>();

            Crop? extCrop = db.Crops.Find(crop.Id);
            if (extCrop == null)
            {
                response.statusCode = HttpStatusCode.BadRequest;
                response.message = "ID not found!";
            }
            else
            {
                // Raw SQL 
                DateTime updateTime = DateTime.Now;
                //var rows = db.Database
                //    .ExecuteSqlInterpolated(
                //        $"update crop set isDeleted = 1, updatedAt = {updateTime.ToString()} where id = {crop.Id}");

                // Method-based syntax
                extCrop.IsDeleted = true;
                extCrop.UpdatedAt = updateTime;
                db.Update(extCrop);

                await db.SaveChangesAsync();

                response.statusCode = HttpStatusCode.OK;
                response.message = "Crop deleted!";
            }

            return response;
        }

        public async Task<ResponseViewModel<List<Crop>>> GetCropsBy(string category)
        {
            ResponseViewModel<List<Crop>> response = new ResponseViewModel<List<Crop>>();

            if (!HttpContext.Session.GetInt32("saveId").HasValue)
            {
                return response;
            }

            SaveFile file = db.SaveFiles.Where(s => s.Id == HttpContext.Session.GetInt32("saveId")).First();
            int? year = file.Year;
            int? curSeason = file.Season;
    
            List<Crop> items = new List<Crop>();
            List<Crop> items2 = new List<Crop>();

            if (category == "all")
            {
                // Query syntax
                items = await (
                    from c in db.Crops 
                    where !c.IsDeleted && c.StartYear <= year
                    select new Crop
                    {
                        Id = c.Id,
                        Name = c.Name
                    }
                ).ToListAsync();

                // Raw SQL syntax
                items2 = await db.Crops
                    .FromSqlRaw($"select * from crop where isDeleted = 0 and startYear <= {year}")
                    .ToListAsync();
            }
            else
            {
                // Query syntax
                items = await (
                    from c in db.Crops
                    join cs in db.CropSeasons
                        on c.Id equals cs.CropId
                    where !c.IsDeleted && c.StartYear <= year &&
                        (category == "season" ?
                            cs.SeasonId == curSeason :
                        category == "ginger" ?
                            cs.SeasonId == 2 :

                            c.Id == 0
                        )
                    select new Crop
                    {
                        Id = c.Id,
                        Name = c.Name
                    }
                ).ToListAsync();

                // Raw SQL syntax
                string query = $"select c.* from crop as c " +
                    $"inner join crop_season as cs on c.id = cs.cropId" +
                    $" where c.isDeleted = 0 and c.startYear <= {year}";
                if (category == "season")
                    query += $" and cs.seasonId = {curSeason}";
                else if (category == "ginger")
                    query += " and cs.seasonId = 2";
                else
                    query += " and c.Id = 0";

                items2 = await db.Crops.FromSqlRaw(query).ToListAsync();
            }

            items = items.OrderBy(c => c.Name).ToList();

            response.data = items;
            response.statusCode = items.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NoContent;
            response.message = items.Count > 0 ? $"{response.statusCode} - {items.Count} data found" : $"{response.statusCode} - No data found";

            return response;
        }
    }
}
