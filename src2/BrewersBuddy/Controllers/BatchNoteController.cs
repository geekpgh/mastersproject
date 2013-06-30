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
    public class BatchNoteController : Controller
    {
        private BrewersBuddyContext db = new BrewersBuddyContext();


        //
        // GET: /BatchNote/Details/5

        public ActionResult Details(int id = 0)
        {
            BatchNote batchnote = db.BatchNotes.Find(id);
            if (batchnote == null)
            {
                return HttpNotFound();
            }
            return View(batchnote);
        }


        //
        // GET: /BatchNote/Edit/5

        public ActionResult Edit(int id = 0)
        {
            BatchNote batchnote = db.BatchNotes.Find(id);
            if (batchnote == null)
            {
                return HttpNotFound();
            }
            return View(batchnote);
        }

        //
        // POST: /BatchNote/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BatchNote batchnote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(batchnote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(batchnote);
        }
}