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
    public class RoomTabController : Controller
    {
        private BookingRoomEntities db = new BookingRoomEntities();

        // GET: RoomTab
        public ActionResult Index()
        {
            return View(db.RoomTabs.ToList());
        }

        // GET: RoomTab/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomTab roomTab = db.RoomTabs.Find(id);
            if (roomTab == null)
            {
                return HttpNotFound();
            }
            return View(roomTab);
        }

        // GET: RoomTab/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomTab/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomNo,RoomName,RoomLevel")] RoomTab roomTab)
        {
            if (ModelState.IsValid)
            {
                db.RoomTabs.Add(roomTab);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roomTab);
        }

        // GET: RoomTab/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomTab roomTab = db.RoomTabs.Find(id);
            if (roomTab == null)
            {
                return HttpNotFound();
            }
            return View(roomTab);
        }

        // POST: RoomTab/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomNo,RoomName,RoomLevel")] RoomTab roomTab)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomTab).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roomTab);
        }

        // GET: RoomTab/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomTab roomTab = db.RoomTabs.Find(id);
            if (roomTab == null)
            {
                return HttpNotFound();
            }
            return View(roomTab);
        }

        // POST: RoomTab/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RoomTab roomTab = db.RoomTabs.Find(id);
            if (db.BookingTabs.Any(b => b.RoomNo == roomTab.RoomNo))
            {
                roomTab.ErrorMessage = "Please delete all the bookings for this Room before deleting Room";
                return View(roomTab);
            }
         else
            { 
            db.RoomTabs.Remove(roomTab);
            db.SaveChanges();
            return RedirectToAction("Index");
            }
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
