﻿using Microsoft.AspNetCore.Mvc;
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
            SaveFile? saveFile = db.SaveFiles.Where(sf => sf.UserId == userId).First();
            if (saveFile != null)
            {
                HttpContext.Session.SetInt32("userId", userId);
                HttpContext.Session.SetInt32("saveId", userId);
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