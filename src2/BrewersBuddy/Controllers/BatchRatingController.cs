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

        public ActionResult Create(int batchId = 0)
        {
            int userId = _userService.GetCurrentUserId();
            if (userId == 0)
                return new HttpUnauthorizedResult();

            Batch batch = _batchService.Get(batchId);
            if (batch == null)
                return new HttpStatusCodeResult(500);

            // You can only rate once!
            BatchRating previousRating = _ratingService.GetUserRatingForBatch(batchId, userId);
            if (previousRating != null)
                return new HttpNotFoundResult();

            ViewBag.BatchId = batchId;
            ViewBag.BatchName = batch.Name;
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
            int userId = _userService.GetCurrentUserId();
            if (userId == 0)
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid)
            {
                Batch batch = _batchService.Get(userRating.BatchId);
                if (batch == null)
                    return new HttpStatusCodeResult(500);

                BatchRating previousRating = _ratingService.GetUserRatingForBatch(userRating.BatchId, userId);
                if (previousRating != null)
                    return new HttpNotFoundResult();

                userRating.UserId = userId;

                _ratingService.Create(userRating);

                return RedirectToAction("Details", "Batch", new { id = userRating.BatchId });
            }

            return View(userRating);
        }
    }
}
