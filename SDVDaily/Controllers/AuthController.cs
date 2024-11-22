using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDVDaily.Models;
using System.Net;

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
            ViewBag.Title = "Login";
            return View();
        }

        [HttpPost]
        public async Task<ResponseViewModel<User>> Login(User user)
        {
            ResponseViewModel<User> response = new ResponseViewModel<User>();

            User? extUser = await db.Users.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
            if (extUser == null)
            {
                response.statusCode = HttpStatusCode.BadRequest;
                response.message = $"Wrong email or password!";
            }
            else
            {
                bool verified = BCrypt.Net.BCrypt.Verify(user.Password, extUser.Password);
                if (!verified)
                {
                    response.statusCode = HttpStatusCode.BadRequest;
                    response.message = $"Wrong email or password!";

                    if (extUser.LoginAttempt == null)
                    {
                        extUser.LoginAttempt = 0;
                    }
                    extUser.LoginAttempt++;
                    extUser.UpdatedAt = DateTime.Now;
                    db.Update(extUser);
                }
                else
                {
                    extUser.UpdatedAt = DateTime.Now;
                    extUser.LastLogin = DateTime.Now;

                    HttpContext.Session.SetInt32("userId", extUser.Id);
                    if (extUser.LastSave == null)
                    {
                        SaveFile? saveFile = db.SaveFiles.Where(sf => sf.UserId == extUser.Id).FirstOrDefault();
                        if (saveFile != null)
                        {
                            HttpContext.Session.SetInt32("saveId", saveFile.Id);
                            HttpContext.Session.SetString("saveName", saveFile?.Name!);
                            extUser.LastSave = saveFile!.Id;
                        }
                    }
                    else
                    {
                        SaveFile saveFile = db.SaveFiles.Where(sf => sf.Id == extUser.LastSave).Single();
                        HttpContext.Session.SetInt32("saveId", saveFile.Id);
                        HttpContext.Session.SetString("saveName", saveFile.Name);
                    }
                    db.Update(extUser);

                    response.statusCode = HttpStatusCode.OK;
                    response.message = $"Login successful!";
                }

                await db.SaveChangesAsync();
            }

            return response;
        }

        public IActionResult Register()
        {
            ViewBag.Title = "Register";
            return View();
        }

        [HttpPost]
        public async Task<ResponseViewModel<User>> Register(User user)
        {
            ResponseViewModel<User> response = new ResponseViewModel<User>();

            User? extUser = await db.Users.Where(u => u.Email == user.Email || u.Username == user.Username).FirstOrDefaultAsync();
            if (extUser != null)
            {
                response.statusCode = HttpStatusCode.BadRequest;
                response.message = $"Username/email already exists!";
            }
            else
            {
				user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

				user.RoleId = 2;
                db.Add(user);
                await db.SaveChangesAsync();

                response.data = user;
                response.statusCode = HttpStatusCode.Created;
                response.message = $"User successfully registered!";
            }

            return response;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
