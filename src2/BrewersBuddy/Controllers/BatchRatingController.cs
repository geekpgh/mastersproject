using BrewersBuddy.Models;
using BrewersBuddy.Services;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

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

        public ActionResult Index(int batchId = 0)
        {
            int userId = _userService.GetCurrentUserId();
            if (userId == 0)
                return new HttpUnauthorizedResult();

            Batch batch = _batchService.Get(batchId);
            if (batch == null)
                return new HttpNotFoundResult();

            if (!batch.CanView(userId))
                return new HttpUnauthorizedResult();

            ViewBag.BatchName = batch.Name;

            IEnumerable<BatchRating> ratings = _ratingService.GetAllForBatch(batchId);

            return PartialView(ratings.ToList());
        }

        public ActionResult Average(int batchId = 0)
        {
            int userId = _userService.GetCurrentUserId();
            if (userId == 0)
                return new HttpUnauthorizedResult();

            Batch batch = _batchService.Get(batchId);
            if (batch == null)
                return new HttpNotFoundResult();

            if (!batch.CanView(userId))
                return new HttpUnauthorizedResult();

            double average = 0;
            if (batch.Ratings != null && batch.Ratings.Count > 0)
                average = batch.Ratings.Average(rating => rating.Rating);

            return Json(average, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(int batchId = 0)
        {
            int userId = _userService.GetCurrentUserId();
            if (userId == 0)
                return new HttpUnauthorizedResult();

            Batch batch = _batchService.Get(batchId);
            if (batch == null)
                return new HttpNotFoundResult();

            bool canRate = batch.CanRate(userId);
            if (!canRate)
                return new HttpStatusCodeResult(403, "You can only rate a batch once");

            ViewBag.BatchId = batchId;
            ViewBag.BatchName = batch.Name;
            ViewBag.Ratings = Enumerable.Range(0, 101)
                .Select(num => new SelectListItem()
                {
                    Text = num.ToString(),
                    Value = num.ToString()
                });

            return PartialView();
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
                    return new HttpNotFoundResult();

                bool canRate = batch.CanRate(userId);
                if (!canRate)
                    return new HttpStatusCodeResult(403, "You can only rate a batch once");

                userRating.UserId = userId;

                _ratingService.Create(userRating);

                return Json(userRating);
            }

            return new HttpStatusCodeResult(500);
        }
    }
}
