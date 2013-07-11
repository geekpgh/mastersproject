using BrewersBuddy.Models;
using BrewersBuddy.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BrewersBuddy.Controllers
{
    [Authorize]
    public class BatchRatingController : Controller
    {
        private readonly IBatchService _batchService;
        private readonly IBatchRatingService _ratingService;
        private readonly IUserService _userService;

        public BatchRatingController(
            IBatchRatingService ratingService,
            IBatchService batchService,
            IUserService userService)
        {
            if (ratingService == null)
                throw new ArgumentNullException("ratingService");
            if (batchService == null)
                throw new ArgumentNullException("batchService");
            if (userService == null)
                throw new ArgumentNullException("userService");

            _ratingService = ratingService;
            _batchService = batchService;
            _userService = userService;
        }

        //
        // GET: /BatchRating/Create
        public ActionResult Create(int batchId)
        {
            int currentUserId = _userService.GetCurrentUserId();
            Batch batch = _batchService.Get(batchId);
            BatchRating previousRating = _ratingService.GetUserRatingForBatch(batchId, currentUserId);

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

            // Create a list from 0 - 100 for all possible rating values
            ViewBag.Ratings = Enumerable.Range(0, 101)
                .Select(num => new SelectListItem()
                {
                    Text = num.ToString(),
                    Value = num.ToString()
                });

            return View();
        }

        //
        // POST: /BatchRating/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BatchRating userRating)
        {
            if (ModelState.IsValid)
            {
                userRating.UserId = _userService.GetCurrentUserId();

                _ratingService.Create(userRating);

                return RedirectToAction("Details", "Batch", new { id = userRating.BatchId });
            }


            return View(userRating);
        }
    }
}
