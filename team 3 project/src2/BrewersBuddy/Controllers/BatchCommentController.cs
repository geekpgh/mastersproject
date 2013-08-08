using BrewersBuddy.Models;
using BrewersBuddy.Services;
using System;
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
                userComment.PostDate = DateTime.Now;

                _commentService.Create(userComment);

                return Json(new
                {
                    Comment = userComment.Comment,
                    UserName = _userService.GetCurrentUser().Identity.Name,
                    PostDate = userComment.PostDate.Value.ToString("f")
                });
            }

            return new HttpStatusCodeResult(500);
        }
    }
}
