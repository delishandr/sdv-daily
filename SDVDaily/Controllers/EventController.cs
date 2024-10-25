using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDVDaily.Models;
using System.Security.Cryptography;

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
                string day = string.Empty;
                switch ((eventDays[0] - 1) / 28)
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
                day += ((eventDays[0] - 1) % 28 + 1).ToString();
                if (eventDays.Count > 1)
                {
                    day += "-" + ((eventDays[eventDays.Count-1] - 1) % 28 + 1).ToString();
                }
                item.Days = day;

                item.CreatedAt = e.CreatedAt;
                item.UpdatedAt = e.UpdatedAt;
                item.IsDeleted = e.IsDeleted;
                items.Add(item);
            }

            ViewBag.Title = "Event List";

            return View(items);
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var mEvent = db.Events.Find(id);
            if (mEvent == null)
            {
                return NotFound();
            }

            EventViewModel vmEvent = new EventViewModel();

            vmEvent.Id = mEvent.Id;
            vmEvent.Name = mEvent.Name;
            vmEvent.Type = mEvent.Type;
            vmEvent.Location = mEvent.Location;
            vmEvent.StartTime = mEvent.StartTime;
            vmEvent.EndTime = mEvent.EndTime;
            vmEvent.Preparation = mEvent.Preparation;

            var eventDays = db.EventDays.Where(ed => ed.EventId == id).Select(ed => ed.Day).ToList();
            string day = string.Empty;
            switch ((eventDays[0] - 1) / 28)
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
            day += ((eventDays[0] - 1) % 28 + 1).ToString();
            if (eventDays.Count > 1)
            {
                day += "-" + ((eventDays[eventDays.Count - 1] - 1) % 28 + 1).ToString();
            }
            vmEvent.Days = day;

            ViewBag.Title = "Detail Event";

            return View(vmEvent);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var mEvent = db.Events.Find(id);
            if (mEvent == null)
            {
                return NotFound();
            }

            EventViewModel vmEvent = new EventViewModel();

            vmEvent.Id = mEvent.Id;
            vmEvent.Name = mEvent.Name;
            vmEvent.Type = mEvent.Type;
            vmEvent.Location = mEvent.Location;
            vmEvent.StartTime = mEvent.StartTime;
            vmEvent.EndTime = mEvent.EndTime;
            vmEvent.Preparation = mEvent.Preparation;

            ViewBag.Title = "Edit Event";

            return View(vmEvent);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Type, Location, StartTime, EndTime, Preparation, CreatedAt")] Event mEvent)
        {
            if (id != mEvent.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    mEvent.UpdatedAt = DateTime.Now;
                    db.Update(mEvent);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (db.Events.Find(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(mEvent);
        }
    }
}
