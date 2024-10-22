using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDVDaily.Models;

namespace SDVDaily.Controllers
{
    public class CropController : Controller
    {
        private DB_SDV_DailyContext db;
        private readonly string imageFolder;

        public CropController(IConfiguration _config, DB_SDV_DailyContext _db)
        {
            db = _db;
            imageFolder = _config["ImageFolder"];
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
                List<int?> seasonIds = cropSeasonList.Select(c => c.SeasonId).ToList();
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

            ViewBag.Title = "Crop List";
            ViewBag.ImageFolder = imageFolder;

			return View(items);
        }

        public IActionResult Detail(int? id)
        {
            CropViewModel crop = new CropViewModel();
            try
            {
                if (id > 0)
                {
                    Crop? findCrop = db.Crops.Where(c => c.Id == id).FirstOrDefault();
                    crop.Id = findCrop.Id;
                    crop.Name = findCrop.Name;

                    crop.CategoryId = findCrop.CategoryId;
                    var category = db.CropCategories.Where(cc => cc.Id.Equals(findCrop.CategoryId)).FirstOrDefault();
                    crop.CategoryName = (category != null ? category.Name : "");

                    crop.GrowthTime = findCrop.GrowthTime;
                    crop.RegrowthTime = findCrop.RegrowthTime;
                    crop.Unirrigated = findCrop.Unirrigated;
                    crop.IsWalkable = findCrop.IsWalkable ?? false;
                    crop.StartYear = findCrop.StartYear;
                    crop.SellPrice = findCrop.SellPrice;
                    crop.Img = findCrop.Img;

                    var cropSeasonList = db.CropSeasons.Where(c => c.CropId.Equals(findCrop.Id)).ToList();
                    List<int?> seasonIds = cropSeasonList.Select(c => c.SeasonId).ToList();
                    crop.SeasonIds = seasonIds.ToArray();

                    foreach (CropSeason cropSeason in cropSeasonList)
                    {
                        crop.Seasons.Add(db.Seasons.Where(s => s.Id.Equals(cropSeason.SeasonId)).Single<Season>());
                    }

                    crop.CreatedAt = findCrop.CreatedAt;
                    crop.UpdatedAt = findCrop.UpdatedAt;
                    crop.IsDeleted = findCrop.IsDeleted;
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

            ViewBag.Title = "Crop Detail";
            ViewBag.ImageFolder = imageFolder;

            return View(crop);
        }
    }
}
