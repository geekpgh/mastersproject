using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrewersBuddy.Models;

namespace BrewersBuddy.Controllers
{
    public class BatchController : Controller
    {
        private BatchDBContext db = new BatchDBContext();

        //
        // GET: /Batch/

        public ActionResult Index()
        {
            return View(db.Batches.ToList());
        }

        //
        // GET: /Batch/Details/5

        public ActionResult Details(int id = 0)
        {
            Batch batch = db.Batches.Find(id);
            if (batch == null)
            {
                return HttpNotFound();
            }
            return View(batch);
        }

        //
        // GET: /Batch/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Batch/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Batch batch)
        {
            if (ModelState.IsValid)
            {
                db.Batches.Add(batch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(batch);
        }

        //
        // GET: /Batch/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Batch batch = db.Batches.Find(id);
            if (batch == null)
            {
                return HttpNotFound();
            }
            return View(batch);
        }

        //
        // POST: /Batch/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Batch batch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(batch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(batch);
        }

        //
        // GET: /Batch/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Batch batch = db.Batches.Find(id);
            if (batch == null)
            {
                return HttpNotFound();
            }
            return View(batch);
        }

        //
        // POST: /Batch/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Batch batch = db.Batches.Find(id);
            db.Batches.Remove(batch);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}