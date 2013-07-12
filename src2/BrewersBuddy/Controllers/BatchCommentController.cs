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

        public ActionResult Create(int batchId = 0)
        {
            int userId = _userService.GetCurrentUserId();
            if (userId == 0)
                return new HttpUnauthorizedResult();

            Batch batch = _batchService.Get(batchId);
            if (batch == null)
                return new HttpStatusCodeResult(500);

            ViewBag.BatchId = batchId;
            ViewBag.BatchName = batch.Name;

            return View();
        }

        //
        // POST: /BatchComment/Create
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
                    return new HttpStatusCodeResult(500);

                userComment.UserId = userId;

                _commentService.Create(userComment);

                return RedirectToAction("Details", "Batch", new { id = userComment.BatchId });
            }

            return View(userComment);
        }
    }
}
