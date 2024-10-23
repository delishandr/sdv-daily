using Microsoft.AspNetCore.Mvc;
using SDVDaily.Models;

namespace SDVDaily.Controllers
{
    public class EventController : Controller
    {
        private DB_SDV_DailyContext db;

        public EventController(IConfiguration _config, DB_SDV_DailyContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            var eventList = db.Events.ToList();
            IList<EventViewModel> items = new List<EventViewModel>();

            foreach (Event e in eventList)
            {
                EventViewModel item = new EventViewModel();
                item.Id = e.Id;
                item.Name = e.Name;
                item.Type = e.Type;
                item.Location = e.Location;
                item.StartTime = e.StartTime;
                item.EndTime = e.EndTime;
                item.Preparation = e.Preparation;

                var eventDays = db.EventDays.Where(ed => ed.EventId == e.Id).Select(ed => ed.Day).ToList();
                foreach (int? ed in eventDays)
                {
                    string day = string.Empty;
                    switch ((ed-1) / 28)
                    {
                        case 0:
                            day += "Spring ";
                            break;
                        case 1:
                            day += "Summer ";
                            break;
                        case 2:
                            day += "Fall ";
                            break;
                        case 3:
                            day += "Winter ";
                            break;
                    }
                    day += ((ed-1) % 28 + 1).ToString();
                    item.Days.Add(day);
                }

                item.CreatedAt = e.CreatedAt;
                item.UpdatedAt = e.UpdatedAt;
                item.IsDeleted = e.IsDeleted;
                items.Add(item);
            }

            ViewBag.Title = "Event List";

            return View(items);
        }
    }
}
