using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BrewersBuddy.Controllers
{
    [Authorize]
    public class BatchRatingController : Controller
    {
        private BrewersBuddyContext db = new BrewersBuddyContext();

        //
        // GET: /BatchRating/Create

        public ActionResult Create(int batchId = 0)
        {
            int currentUserId = ControllerUtils.GetCurrentUserId(User);
            Batch batch = db.Batches.Find(batchId);

            BatchRating previousRating = db.BatchRatings
                .Where(r => r.BatchId == batchId && r.UserId == currentUserId)
                .FirstOrDefault();

            if (previousRating != null)
            {
                // You can only rate a batch once
                // TODO - Do something nicer here... this is kind of jarring
                // and no explanation is given in the UI as to why a user can't rate
                return RedirectToAction("Details", "Batch", new { id = batchId });
            }

            if (batch == null)
            {
                // I'm not sure if this makes sense here, but keeping
                // it for the time being - S. Platz
                HttpNotFound();
            }

            BatchRating rating = new BatchRating()
            {
                BatchId = batchId,
                Batch = batch
            };

            // Create a list from 0 - 100 for all possible rating values
            ViewBag.Ratings = Enumerable.Range(0, 101)
                .Select(num => new SelectListItem() 
                { 
                    Text = num.ToString(),
                    Value = num.ToString() 
                });

            return View(rating);
        }

        //
        // POST: /BatchRating/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BatchRating rating)
        {
            if (ModelState.IsValid)
            {
                rating.UserId = ControllerUtils.GetCurrentUserId(User);

                db.BatchRatings.Add(rating);
                db.SaveChanges();
                return RedirectToAction("Details", "Batch", new { id = rating.BatchId });
            }


            return View(rating);
        }
    }
}
