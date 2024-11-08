using Microsoft.AspNetCore.Mvc;
using SDVDaily.Models;

namespace SDVDaily.Controllers
{
    public class AuthController : Controller
    {
        private DB_SDV_DailyContext db;
        public AuthController(DB_SDV_DailyContext _db)
        {
            db = _db;
        }
        public IActionResult Login()
        {
            int userId = 1; // hardcode
            SaveFile? saveFile = db.SaveFiles.Where(sf => sf.UserId == userId).ToList()[0];

            HttpContext.Session.SetInt32("userId", userId);
            HttpContext.Session.SetString("saveName", saveFile?.Name!);
            HttpContext.Session.SetInt32("saveDay", (int)saveFile?.Day!);
            HttpContext.Session.SetInt32("saveSeason", (int)saveFile?.Season!);
            HttpContext.Session.SetInt32("saveYear", (int)saveFile?.Year!);
            HttpContext.Session.SetInt32("hasPet", saveFile?.HasPet! == true ? 1 : 0);
            HttpContext.Session.SetInt32("hasFarmAnimals", saveFile?.HasFarmAnimals! == true ? 1 : 0);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
