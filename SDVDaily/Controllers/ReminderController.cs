using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDVDaily.Models;
using System.Net;

namespace SDVDaily.Controllers
{
    public class ReminderController : Controller
    {
        private DB_SDV_DailyContext db;

        public ReminderController(DB_SDV_DailyContext _db)
        {
            db = _db;
        }

        [HttpGet]
        public IActionResult Add()
        {
            if (!HttpContext.Session.GetInt32("saveId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

            SaveFile save = db.SaveFiles.Where(sf => sf.Id == HttpContext.Session.GetInt32("saveId")).Single();

            List<Day> days = db.Days.ToList();
            ViewBag.Seasons = db.Seasons.ToList();
            ViewBag.DaysLeft = days.Take(4);
            ViewBag.DaysRight = days.Skip(4);
            ViewBag.NextDay = save.Day == 28 ? 1 : save.Day + 1;
            ViewBag.NextSeason = save.Day == 28 ? (save.Season == 4 ? 1 : save.Season + 1) : save.Season;

            ViewBag.Title = "Add Reminder";

            return View();
        }

        [HttpPost]
        public async Task<ResponseViewModel<Reminder>> Add(ReminderViewModel data)
        {
            ResponseViewModel<Reminder> response = new ResponseViewModel<Reminder>();

            SaveFile? save = db.SaveFiles.Find(HttpContext.Session.GetInt32("saveId"));

            if (save != null)
            {
                Reminder reminder = new Reminder();
                reminder.Description = data.Description;
                reminder.SaveId = save.Id;

                switch (data.RemindType)
                {
                    case "day":
                        int remindDay = save.Day + data.NextRemindDay;
                        int remindSeason = save.Season;
                        int remindYear = save.Year;
                        while (remindDay > 28)
                        {
                            remindDay -= 28;
                            remindSeason++;
                            if (remindSeason > 4)
                            {
                                remindSeason = 1;
                                remindYear++;
                            }
                        }
                        reminder.NextRemind = remindDay;
                        reminder.NextRemindSeason = remindSeason;
                        reminder.NextRemindYear = remindYear;

                        break;
                    case "date":
                        reminder.NextRemind = data.NextRemind;
                        reminder.NextRemindSeason = data.NextRemindSeason;
                        reminder.NextRemindYear = save.Year;

                        if (data.NextRemindSeason < save.Season
                            || (data.NextRemindSeason == save.Season && data.NextRemind < save.Day))
                            reminder.NextRemindYear++;
                        break;
                    default:
                        break;
                }
                db.Add(reminder);
                db.SaveChanges();

                ReminderRepeat repeat;
                switch (data.FreqType)
                {
                    case "weekly":
                        repeat = new ReminderRepeat();
                        repeat.ReminderId = reminder.Id;
                        repeat.Day = reminder.NextRemind % 7;
                        if (repeat.Day == 0)
                            repeat.Day = 7;

                        db.Add(repeat);

                        break;
                    case "daily":
                        for (int i = 1; i <= 7; i++)
                        {
                            repeat = new ReminderRepeat();
                            repeat.ReminderId = reminder.Id;
                            repeat.Day = i;

                            db.Add(repeat);
                        }
                        break;
                    case "custom":
                        foreach (int i in data.Frequency)
                        {
                            repeat = new ReminderRepeat();
                            repeat.ReminderId = reminder.Id;
                            repeat.Day = i;

                            db.Add(repeat);
                        }
                        break;
                    default: break; // once
                }

                await db.SaveChangesAsync();

                response.statusCode = HttpStatusCode.Created;
                response.message = "Reminder added!";
            }

            return response;
        }
    }
}
