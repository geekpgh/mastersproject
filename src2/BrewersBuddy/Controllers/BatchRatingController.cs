using BrewersBuddy.Models;
using BrewersBuddy.Services;
using System;
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

                // Make sure the user didn't rate the batch previously
                BatchRating previousRating = _ratingService.GetUserRatingForBatch(userRating.BatchId, userId);
                if (previousRating != null)
                    return new HttpNotFoundResult();

                userRating.UserId = userId;

                _ratingService.Create(userRating);

                return Json(userRating);
            }

            return new HttpStatusCodeResult(500);
        }
    }
}
