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
            var cropList = db.Crops.ToList();
            IList<CropViewModel> items = new List<CropViewModel>();

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
                item.IsWalkable = crop.IsWalkable ?? false;
                item.StartYear = crop.StartYear;
                item.SellPrice = crop.SellPrice;
                item.Img = crop.Img;

                var cropSeasonList = db.CropSeasons.Where(c => c.CropId.Equals(crop.Id)).ToList();
                string seasons = string.Empty;
                foreach (CropSeason cropSeason in cropSeasonList)
                {
                    string? season = db.Seasons.Where(s => s.Id.Equals(cropSeason.SeasonId)).Single<Season>().Name;
                    seasons += season + ", ";
                }
                item.Seasons = seasons.Substring(0, seasons.Length - 2);

                item.CreatedAt = crop.CreatedAt;
                item.UpdatedAt = crop.UpdatedAt;
                item.IsDeleted = crop.IsDeleted;

                items.Add(item);
            }

            ViewBag.Title = "Crop List";

			return View(items);
        }

        public IActionResult Detail(int? id)
        {
            Crop? crop = new Crop();
            try
            {
                if (id > 0)
                {
                    crop = (
                        from c in db.Crops
                        join cc in db.CropCategories
                            on c.CategoryId equals cc.Id into HaveCategory
                        from cc in HaveCategory.DefaultIfEmpty()
                        where !c.IsDeleted && c.Id == id
                        select new Crop
                        {
                            Id = c.Id,
                            Name = c.Name,

                            CategoryId = c.CategoryId,
                            //CategoryName = cc.Name,

                            GrowthTime = c.GrowthTime,
                            RegrowthTime = c.RegrowthTime,
                            Unirrigated = c.Unirrigated,
                            IsWalkable = c.IsWalkable,
                            StartYear = c.StartYear,

                            //Seasons = (
                            //    from cs in db.CropSeasons
                            //    join s in db.Seasons
                            //        on cs.SeasonId equals s.Id
                            //    where cs.CropId == c.Id
                            //    select new CropSeason
                            //    {
                            //        Id = cs.Id,
                            //        SeasonId = cs.SeasonId,
                            //        SeasonName = s.Name
                            //    }
                            //).ToList(),

                            CreatedAt = c.CreatedAt,
                            UpdatedAt = c.UpdatedAt,
                            IsDeleted = c.IsDeleted
                        }
                    ).FirstOrDefault();
                }
                else if (id == 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return View(crop);
        }
    }
}
