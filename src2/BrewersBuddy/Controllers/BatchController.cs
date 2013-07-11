using BrewersBuddy.Models;
using BrewersBuddy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BrewersBuddy.Controllers
{
    [Authorize]
    public class BatchController : Controller
    {
        private readonly IBatchService _batchService;
        private readonly IBatchNoteService _noteService;
        private readonly IBatchRatingService _ratingService;
        private readonly IUserService _userService;

        public BatchController(
            IBatchService batchService,
            IBatchNoteService noteService,
            IBatchRatingService ratingService,
            IUserService userService)
        {
            if (batchService == null)
                throw new ArgumentNullException("batchService");
            if (noteService == null)
                throw new ArgumentNullException("noteService");
            if (ratingService == null)
                throw new ArgumentNullException("ratingService");
            if (userService == null)
                throw new ArgumentNullException("userService");

            _batchService = batchService;
            _noteService = noteService;
            _ratingService = ratingService;
            _userService = userService;
        }

        //
        // GET: /Batch/
        public ActionResult Index()
        {
            int currentUserId = _userService.GetCurrentUserId();
            IEnumerable<Batch> batches = _batchService.GetAllForUser(currentUserId);
            return View(batches);
        }


        //
        // GET: /Batch/Details/5
        public ActionResult Details(int id = 0)
        {
            Batch batch = _batchService.Get(id);
            if (batch == null)
            {
                return HttpNotFound();
            }

            int currentUserId = _userService.GetCurrentUserId();

            ViewBag.UserRating = _ratingService.GetUserRatingForBatch(id, currentUserId);

            if (batch.Ratings.Count > 0)
                ViewBag.AverageRating = batch.Ratings.Average(rating => rating.Rating);
            else
                ViewBag.AverageRating = 0;

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
                batch.OwnerId = _userService.GetCurrentUserId();

                _batchService.Create(batch);

                return RedirectToAction("Index");
            }
            return View(batch);
        }


        //
        // GET: /Batch/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Batch batch = _batchService.Get(id);
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
                _batchService.Update(batch);
                return RedirectToAction("Details/" + batch.BatchId);
            }
            return View(batch);
        }


        //
        // GET: /Batch/Delete/5
        public ActionResult Delete(int id = 0)
        {
            Batch batch = _batchService.Get(id);
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
            Batch batch = _batchService.Get(id);
            _batchService.Delete(batch);
            return RedirectToAction("Index");
        }


        //Custom NON CRUD actions
        public ActionResult AddAction(int id = 0)
        {
            Batch batch = _batchService.Get(id);

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
        public ActionResult AddAction(BatchAction model)
        {
            if (ModelState.IsValid)
            {
                //Add the date
                model.ActionDate = DateTime.Now;
                model.PerformerId = _userService.GetCurrentUserId();

                //Associate the batch with the action
                int batchId = (int)Session["CurrentBatchId"];
                Batch batch = _batchService.Get(batchId);

                _batchService.AddAction(batch, model);

                return RedirectToAction("Details/" + batch.BatchId);
            }
            return View(model);

        }

        public ActionResult AddNote(int id = 0)
        {
            Batch batch = _batchService.Get(id);

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
                note.AuthorId = _userService.GetCurrentUserId();

                //Associate the batch with the action
                int batchId = (int)Session["CurrentBatchId"];
                Batch batch = _batchService.Get(batchId);

                _batchService.AddNote(batch, note);

                return RedirectToAction("Details/" + batch.BatchId);
            }

            return View(note);
        }

        public ActionResult AddMeasurement(int id = 0)
        {
            Batch batch = _batchService.Get(id);

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
                Batch batch = _batchService.Get(batchId);

                _batchService.AddMeasurement(batch, measurement);

                return RedirectToAction("Details/" + batch.BatchId);
            }

            return View(measurement);
        }

        //
        // GET: /Batch/DeleteNote/5
        public ActionResult DeleteNote(int noteId = 0, int batchId = 0)
        {
            Batch batch = _batchService.Get(batchId);
            if (batch == null)
            {
                return HttpNotFound();
            }
            Session["CurrentBatchId"] = batch.BatchId;

            BatchNote note = _noteService.Get(noteId);
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
        public ActionResult DeleteNoteConfirmed(int noteId = 0)
        {
            BatchNote note = _noteService.Get(noteId);

            _noteService.Delete(note);

            //Associate the batch with the note
            int batchId = (int)Session["CurrentBatchId"];

            return RedirectToAction("Details/" + batchId);
        }


        public ActionResult EditNote(int noteId = 0, int batchId = 0)
        {
            Batch batch = _batchService.Get(batchId);
            if (batch == null)
            {
                return HttpNotFound();
            }
            Session["CurrentBatchId"] = batch.BatchId;

            BatchNote note = _noteService.Get(noteId);
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
                _noteService.Update(note);

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
            IEnumerable<BatchRating> ratings = _ratingService.GetAllForBatch(id);

            return View(ratings);
        }
    }
}
