using BrewersBuddy.Models;
using BrewersBuddy.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BrewersBuddy.Controllers
{
    [Authorize]
    public class BatchCommentController : Controller
    {
        private readonly IBatchService _batchService;
        private readonly IBatchCommentService _commentService;
        private readonly IUserService _userService;

        public BatchCommentController(
            IBatchCommentService commentService,
            IBatchService batchService,
            IUserService userService)
        {
            if (commentService == null)
                throw new ArgumentNullException("commentService");
            if (batchService == null)
                throw new ArgumentNullException("batchService");
            if (userService == null)
                throw new ArgumentNullException("userService");

            _commentService = commentService;
            _batchService = batchService;
            _userService = userService;
        }

        //
        // POST: /BatchRating/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BatchComment userComment)
        {
            int userId = _userService.GetCurrentUserId();
            if (userId == 0)
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid)
            {
                Batch batch = _batchService.Get(userComment.BatchId);
                if (batch == null)
                    return new HttpNotFoundResult();

                bool canRate = batch.CanView(userId);
                if (!batch.CanView(userId))
                    return new HttpUnauthorizedResult();

                userComment.UserId = userId;

                _commentService.Create(userComment);

                return Json(userComment);
            }

            return new HttpStatusCodeResult(500);
        }
    }
}
