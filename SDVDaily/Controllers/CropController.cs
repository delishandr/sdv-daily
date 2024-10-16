using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDVDaily.Models;

namespace SDVDaily.Controllers
{
    public class CropController : Controller
    {
        private DB_SDV_DailyContext db;

        public CropController(DB_SDV_DailyContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            List<Crop> cropList = (
                from c in db.Crops
                join cc in db.CropCategories
                    on c.CategoryId equals cc.Id into HaveCategory
                    from cc in HaveCategory.DefaultIfEmpty()
                where !c.IsDeleted
                select new Crop
                {
                    Id = c.Id,
                    Name = c.Name,

                    CategoryId = c.CategoryId,
                    CategoryName = cc.Name,

                    GrowthTime = c.GrowthTime,
                    RegrowthTime = c.RegrowthTime,
                    Unirrigated = c.Unirrigated,
                    IsWalkable = c.IsWalkable,
                    StartYear = c.StartYear,

                    Seasons = (
                        from cs in db.CropSeasons
                        join s in db.Seasons
                            on cs.SeasonId equals s.Id
                        where cs.CropId == c.Id
                        select new CropSeason
                        {
                            Id = cs.Id,
                            SeasonId = cs.SeasonId,
                            SeasonName = s.Name
                        }
                    ).ToList(),

                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    IsDeleted = c.IsDeleted
                }
            ).ToList();

			return View(cropList);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Crops = await db.Crops.ToListAsync();

            return View();
        }
    }
}
