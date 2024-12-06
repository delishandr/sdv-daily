﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDVDaily.Models;
using System.Net;
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
        public async Task<IActionResult> Index()
        {
            // Method-based syntax
            var query = db.Events.Where(e => !e.IsDeleted);

            // Query syntax
            var query1 = from e in db.Events where !e.IsDeleted select e;

            // Raw SQL syntax
            var query2 = db.Events
                .FromSqlRaw("select * from event where isDeleted = 0");

            List<Event> events = await query.ToListAsync();

            List<EventViewModel> items = new List<EventViewModel>();

            foreach (Event e in events)
            {
                EventViewModel item = new EventViewModel();
                item.Id = e.Id;
                item.Name = e.Name;
                item.Type = e.Type;
                item.Location = e.Location;
                item.StartTime = e.StartTime;
                item.EndTime = e.EndTime;
                item.Preparation = e.Preparation;

                List<EventDay> eventDays = db.EventDays.Where(ed => ed.EventId == e.Id).ToList();
                string day = string.Empty;
                day += db.Seasons.Where(s => s.Id == eventDays[0].Season).Select(s => s.Name).First() + " ";
                day += (eventDays[0].Day).ToString();
                if (eventDays.Count > 1)
                {
                    day += "-" + (eventDays[eventDays.Count - 1].Day).ToString();
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
        public async Task<IActionResult> Detail(int id)
        {
            // Method-based syntax
            var query = db.Events
                .Where(e => !e.IsDeleted && e.Id == id);

            // Query syntax
            var query1 = from e in db.Events where !e.IsDeleted && e.Id == id select e;

            // Raw SQL syntax
            var query2 = db.Events
                .FromSqlRaw($"select * from event where isDeleted = 0 and id = {id}");

            Event? mEvent = await query.FirstOrDefaultAsync();

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

            var eventDays = db.EventDays.Where(ed => ed.EventId == id).ToList();
            string day = string.Empty;
            day += db.Seasons.Where(s => s.Id == eventDays[0].Season).Select(s => s.Name).First() + " ";
            day += (eventDays[0].Day).ToString();
            if (eventDays.Count > 1)
            {
                day += "-" + (eventDays[eventDays.Count - 1].Day).ToString();
            }
            vmEvent.Days = day;

            ViewBag.Title = "Event Detail";

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

            ViewBag.Title = "Edit Event";

            return View(mEvent);
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
                    DateTime updateTime = DateTime.Now;

                    // Raw SQL syntax
                    //var rows = db.Database
                    //    .ExecuteSqlInterpolated(
                    //        $"update event set name = {mEvent.Name}, type = {mEvent.Type}, location = {mEvent.Location}, startTime = {mEvent.StartTime}, endTime = {mEvent.EndTime}, preparation = {mEvent.Preparation}, updatedAt = {updateTime.ToString()} where id = {id}");


                    // Method-based syntax
                    mEvent.UpdatedAt = updateTime;
                    db.Update(mEvent);
                    await db.SaveChangesAsync();

                    HttpContext.Session.SetString("infoMsg", "Event updated!");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (db.Events.Find(id) == null)
                    {
                        HttpContext.Session.SetString("errMsg", "Error: ID not found!");
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

        [HttpGet] 
        public IActionResult Delete(int id)
        {
            ViewBag.Id = id;
            ViewBag.Title = "Delete Event";

            return View();
        }

        [HttpPost]
        public async Task<ResponseViewModel<Event>> Delete(Event mEvent)
        {
            ResponseViewModel<Event> response = new ResponseViewModel<Event>();

            Event? extEvent = db.Events.Find(mEvent.Id);
            if (extEvent == null)
            {
                response.statusCode = HttpStatusCode.BadRequest;
                response.message = "ID not found!";
            }
            else
            {
                DateTime updateTime = DateTime.Now;

                // Raw SQL syntax
                //var rows = db.Database
                //    .ExecuteSqlInterpolated(
                //        $"update event set isDeleted = 1, updatedAt = {updateTime.ToString()} where id = {mEvent.Id}");

                // Method-based syntax
                extEvent.IsDeleted = true;
                extEvent.UpdatedAt = DateTime.Now;
                db.Update(extEvent);

                await db.SaveChangesAsync();

                response.statusCode = HttpStatusCode.OK;
                response.message = "Event deleted!";
            }
            

            return response;
        }
    }
}
