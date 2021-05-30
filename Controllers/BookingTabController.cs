using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookMeetingRoom.Models;

namespace BookMeetingRoom.Controllers
{
    public class BookingTabController : Controller
    {
        private BookingRoomEntities db = new BookingRoomEntities();

        // GET: BookingTab
        public ActionResult Index()
        {
            var bookingTabs = db.BookingTabs.Include(b => b.RoomTab);
            return View(bookingTabs.ToList());
        }

        // GET: BookingTab/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingTab bookingTab = db.BookingTabs.Find(id);
            if (bookingTab == null)
            {
                return HttpNotFound();
            }
            return View(bookingTab);
        }

        // GET: BookingTab/Create
        public ActionResult Create(string v)
        {
            ViewBag.RoomNo = new SelectList(db.RoomTabs, "RoomNo", "RoomName");
            return View();
        }

        // POST: BookingTab/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string v, string v1, [Bind(Include = "BookingId,RoomNo,MeetingDt,StartTime,EndTime,Subject")] BookingTab bookingTab)
        {
            if (bookingTab.MeetingDt < DateTime.Today)
            {
                ModelState.AddModelError("MeetingDt", "Meeting Date cannot be past dated");
                ViewBag.RoomNo = new SelectList(db.RoomTabs, "RoomNo", "RoomName", bookingTab.RoomNo);
                return View(bookingTab);

            }
            if (bookingTab.StartTime > bookingTab.EndTime)
            {
                ModelState.AddModelError("StartTime", "Start Time must be before End Time");
                ViewBag.RoomNo = new SelectList(db.RoomTabs, "RoomNo", "RoomName", bookingTab.RoomNo);
                return View(bookingTab);
            }
            if (db.BookingTabs.Any(b => b.RoomNo == bookingTab.RoomNo
                     && b.MeetingDt == bookingTab.MeetingDt
                     && b.StartTime <= bookingTab.EndTime
                     && b.EndTime >= bookingTab.StartTime))
            {
                bookingTab.ErrorMessage = "Room is alreaday booked for this Timeslot";
                ViewBag.RoomNo = new SelectList(db.RoomTabs, "RoomNo", "RoomName", bookingTab.RoomNo);
                // ModelState.AddModelError("StartDate", "You have already booked");
                return View(bookingTab);

                //    DisplaySuccessMessage("Meeting room is already booked");
            }
            else
            {
                if (ModelState.IsValid)
            {
                db.BookingTabs.Add(bookingTab);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            }

            ViewBag.RoomNo = new SelectList(db.RoomTabs, "RoomNo", "RoomName", bookingTab.RoomNo);
            return View(bookingTab);
        }

        // GET: BookingTab/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingTab bookingTab = db.BookingTabs.Find(id);
            if (bookingTab == null)
            {
                return HttpNotFound();
            }
            if (bookingTab.MeetingDt < DateTime.Today)
            {
                ModelState.AddModelError("MeetingDt", "You cannot edit past dated booking");
                ViewBag.RoomNo = new SelectList(db.RoomTabs, "RoomNo", "RoomName", bookingTab.RoomNo);
                return View(bookingTab);
            }
            ViewBag.RoomNo = new SelectList(db.RoomTabs, "RoomNo", "RoomName", bookingTab.RoomNo);
            return View(bookingTab);
        }

        // POST: BookingTab/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingId,RoomNo,MeetingDt,StartTime,EndTime,Subject")] BookingTab bookingTab)
        {
            if (bookingTab.MeetingDt < DateTime.Today)
            {
                ModelState.AddModelError("MeetingDt", "Meeting Date cannot be past dated");
                ViewBag.RoomNo = new SelectList(db.RoomTabs, "RoomNo", "RoomName", bookingTab.RoomNo);
                return View(bookingTab);
            }
            if (bookingTab.StartTime > bookingTab.EndTime)
            {
                ModelState.AddModelError("StartTime", "Start Time must be before End Time");
                ViewBag.RoomNo = new SelectList(db.RoomTabs, "RoomNo", "RoomName", bookingTab.RoomNo);
                return View(bookingTab);
            }
            if (db.BookingTabs.Any(b => b.BookingId != bookingTab.BookingId
                     && b.RoomNo == bookingTab.RoomNo
                     && b.MeetingDt == bookingTab.MeetingDt
                     && b.StartTime <= bookingTab.EndTime
                     && b.EndTime >= bookingTab.StartTime))
            {
                bookingTab.ErrorMessage = "Room is alreaday booked for this Timeslot";
                // ModelState.AddModelError("StartDate", "You have already booked");
                ViewBag.RoomNo = new SelectList(db.RoomTabs, "RoomNo", "RoomName", bookingTab.RoomNo);
                return View(bookingTab);

                //    DisplaySuccessMessage("Meeting room is already booked");
            }
            else
            {
                if (ModelState.IsValid)
            {
                db.Entry(bookingTab).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            }
            ViewBag.RoomNo = new SelectList(db.RoomTabs, "RoomNo", "RoomName", bookingTab.RoomNo);
            return View(bookingTab);
        }

        // GET: BookingTab/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingTab bookingTab = db.BookingTabs.Find(id);
            if (bookingTab == null)
            {
                return HttpNotFound();
            }
            return View(bookingTab);
        }

        // POST: BookingTab/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookingTab bookingTab = db.BookingTabs.Find(id);
            db.BookingTabs.Remove(bookingTab);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
