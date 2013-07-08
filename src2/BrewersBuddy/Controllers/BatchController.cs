﻿using System.Data;
using System.Linq;
using System.Web.Mvc;
using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Web.Security;
using WebMatrix.WebData;
using System.Data.Entity;

namespace BrewersBuddy.Controllers
{
    [Authorize]
    public class BatchController : Controller
    {
        private BrewersBuddyContext db = new BrewersBuddyContext();

        //
        // GET: /Batch/


        public ActionResult Index()
        {
            int currentUserId = ControllerUtils.getCurrentUserId(User);

            //Get only the batches for the current user
            var owndedBatches = from batch in db.Batches
                                where (batch.OwnerId.Equals(currentUserId))
                                select batch;

            return View(owndedBatches.ToList());
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
                //Set the start date to now
                batch.StartDate = DateTime.Now;
                //Tie the object to the user
                batch.OwnerId = ControllerUtils.getCurrentUserId(User);

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

            //SelectType(batch.Type);

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
                return RedirectToAction("Details/" + batch.BatchId);
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


        //Custom NON CRUD actions
        public ActionResult AddAction(int id = 0)
        {
            Batch batch = db.Batches.Find(id);

            if (batch == null)
            {
                return HttpNotFound();
            }

            //Store away the batch for the callback
            Session["CurrentBatchId"] = batch.BatchId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAction(BatchAction action)
        {
            if (ModelState.IsValid)
            {
                //Add the date
                action.ActionDate = DateTime.Now;
                action.PerformerId = ControllerUtils.getCurrentUserId(User);

                //Associate the batch with the action
                int batchId = (int)Session["CurrentBatchId"];
                Batch batch = db.Batches.Find(batchId);

                db.Entry(action).State = EntityState.Added;
                batch.Actions.Add(action);

                db.SaveChanges();
                return RedirectToAction("Details/" + batch.BatchId);
            }
            return View(action);
       
       }

        public ActionResult AddNote(int id = 0)
        {
            Batch batch = db.Batches.Find(id);

            if (batch == null)
            {
                return HttpNotFound();
            }

            Session["CurrentBatchId"] = batch.BatchId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNote(BatchNote note)
        {
            if (ModelState.IsValid)
            {
                //Add the date
                note.AuthorDate = DateTime.Now;
                note.AuthorId = ControllerUtils.getCurrentUserId(User);

                //Associate the batch with the action
                int batchId = (int)Session["CurrentBatchId"];
                Batch batch = db.Batches.Find(batchId);

                db.Entry(note).State = EntityState.Added;
                batch.Notes.Add(note);
                db.SaveChanges();
                return RedirectToAction("Details/" + batch.BatchId);
            }

            return View(note);
        }

        public ActionResult AddMeasurement(int id = 0)
        {
            Batch batch = db.Batches.Find(id);

            if (batch == null)
            {
                return HttpNotFound();
            }

            Session["CurrentBatchId"] = batch.BatchId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMeasurement(Measurement measurement)
        {
            if (ModelState.IsValid)
            {
                //Add the date
                measurement.MeasurementDate = DateTime.Now;

                //Associate the batch with the measurement
                int batchId = (int)Session["CurrentBatchId"];
                Batch batch = db.Batches.Find(batchId);
                measurement.Batch = batch;

                db.Entry(measurement).State = EntityState.Added;
                batch.Measurements.Add(measurement);
                db.SaveChanges();
                return RedirectToAction("Details/" + batch.BatchId);
            }

            return View(measurement);
        }

        //
        // GET: /Batch/DeleteNote/5

        public ActionResult DeleteNote(int idNote = 0, int idBatch = 0)
        {
            Batch batch = db.Batches.Find(idBatch);
            if (batch == null)
            {
                return HttpNotFound();
            }
            Session["CurrentBatchId"] = batch.BatchId;

            BrewersBuddyContext db2 = new BrewersBuddyContext();

            BatchNote note = db.BatchNotes.Find(idNote);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        //
        // POST: /Batch/DeleteNote/5

        [HttpPost, ActionName("DeleteNote")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteNoteConfirmed(int idNote = 0)
        {
            BatchNote note = db.BatchNotes.Find(idNote);

            db.BatchNotes.Remove(note);
            db.SaveChanges();

            //Associate the batch with the note
            int batchId = (int)Session["CurrentBatchId"];

            return RedirectToAction("Details/" + batchId);
        }


        public ActionResult EditNote(int idNote = 0, int idBatch = 0)
        {
            Batch batch = db.Batches.Find(idBatch);
            if (batch == null)
            {
                return HttpNotFound();
            }
            Session["CurrentBatchId"] = batch.BatchId;

            BrewersBuddyContext db2 = new BrewersBuddyContext();

            BatchNote note = db2.BatchNotes.Find(idNote);
            if (note == null)
            {
                return HttpNotFound();
            }

            return View(note);
        }


        //
        // POST: /Batch/EditNote/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditNote(BatchNote note)
        {
            if (ModelState.IsValid)
            {
                BrewersBuddyContext db2 = new BrewersBuddyContext();
                db2.Entry(note).State = EntityState.Modified;
                db2.SaveChanges();

                //Associate the batch with the note
                int batchId = (int)Session["CurrentBatchId"];

                return RedirectToAction("Details/" + batchId);
            }
            return View(note);
        }


        //
        // GET: /Batch/Ratings/5

        public ActionResult Ratings(int id)
        {
            IEnumerable<BatchRating> ratings = db.BatchRatings
                .Where(r => r.BatchId == id);

            return View(ratings);
        }

        //Clenup and disposal code
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
