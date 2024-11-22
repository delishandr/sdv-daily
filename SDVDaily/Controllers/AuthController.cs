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
            int userId = 2; // hardcode
            HttpContext.Session.SetInt32("userId", userId);
            SaveFile? saveFile = db.SaveFiles.Where(sf => sf.UserId == userId).FirstOrDefault();
            if (saveFile != null)
            {
                HttpContext.Session.SetInt32("saveId", saveFile.Id);
                HttpContext.Session.SetString("saveName", saveFile?.Name!);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
