using BrewersBuddy.Filters;
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
        private readonly IBatchRatingService _ratingService;
        private readonly IUserService _userService;

        public BatchController(
            IBatchService batchService,
            IBatchRatingService ratingService,
            IUserService userService)
        {
            if (batchService == null)
                throw new ArgumentNullException("batchService");
            if (ratingService == null)
                throw new ArgumentNullException("ratingService");
            if (userService == null)
                throw new ArgumentNullException("userService");

            _batchService = batchService;
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

            BatchRating userRating = _ratingService.GetUserRatingForBatch(id, currentUserId);
            if (userRating != null)
                ViewBag.UserRating = userRating.Rating;
            else
                ViewBag.UserRating = "N/A";

            if (batch.Ratings.Count > 0)
                ViewBag.AverageRating = batch.Ratings.Average(rating => rating.Rating);
            else
                ViewBag.AverageRating = "N/A";

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
            CheckAuthorization(id);
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
            CheckAuthorization(batch.BatchId);

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
            CheckAuthorization(id);
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
            CheckAuthorization(id);
            Batch batch = _batchService.Get(id);
            _batchService.Delete(batch);
            return RedirectToAction("Index");
        }


        //
        // GET: /Batch/Ratings/5
        public ActionResult Ratings(int id)
        {
            IEnumerable<BatchRating> ratings = _ratingService.GetAllForBatch(id);

            return View(ratings);
        }

        private void CheckAuthorization(int batchId)
        {
            int currentUser = _userService.GetCurrentUserId();
            int batchOwner = _batchService.Get(batchId).OwnerId;

            if (batchOwner != currentUser)
            {
                throw new UnauthorizedAccessException("Cannot alter data you do not own.");
            }
        }
    }
}
