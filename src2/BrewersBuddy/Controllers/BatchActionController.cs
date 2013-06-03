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
    public class BatchActionController : Controller
    {
        private BrewersBuddyContext db = new BrewersBuddyContext();

        public ActionResult SelectType()
        {
            ViewBag.ActionType = ControllerUtils.getSelectionForEnum<ActionType>();
            return View();
        }

        //
        // GET: /BatchAction/

        public ActionResult Index()
        {
            return View(db.BatchActions.ToList());
        }

        //
        // GET: /BatchAction/Details/5

        public ActionResult Details(int id = 0)
        {
            BatchAction batchaction = db.BatchActions.Find(id);
            if (batchaction == null)
            {
                return HttpNotFound();
            }
            return View(batchaction);
        }

        //
        // GET: /BatchAction/Create

        public ActionResult Create()
        {
            //Populate the action type dropdown
            SelectType();

            return View();
        }

        //
        // POST: /BatchAction/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BatchAction batchAction)
        {
            if (ModelState.IsValid)
            {
                batchAction.PerformerId = ControllerUtils.getCurrentUserId(User);
                db.BatchActions.Add(batchAction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(batchAction);
        }

        //
        // GET: /BatchAction/Edit/5

        public ActionResult Edit(int id = 0)
        {
            //Populate the action type dropdown
            SelectType();

            BatchAction batchaction = db.BatchActions.Find(id);
            if (batchaction == null)
            {
                return HttpNotFound();
            }
            return View(batchaction);
        }

        //
        // POST: /BatchAction/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BatchAction batchaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(batchaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(batchaction);
        }

        //
        // GET: /BatchAction/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BatchAction batchaction = db.BatchActions.Find(id);
            if (batchaction == null)
            {
                return HttpNotFound();
            }
            return View(batchaction);
        }

        //
        // POST: /BatchAction/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BatchAction batchaction = db.BatchActions.Find(id);
            db.BatchActions.Remove(batchaction);
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