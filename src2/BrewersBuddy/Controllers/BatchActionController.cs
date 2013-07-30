using BrewersBuddy.Models;
using BrewersBuddy.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BrewersBuddy.Controllers
{
    [Authorize]
    public class BatchActionController : Controller
    {
        private readonly IBatchActionService _actionService;
        private readonly IUserService _userService;
        private readonly IBatchService _batchService;

        public BatchActionController(
            IBatchActionService actionService,
            IBatchService batchService,
            IUserService userService)
        {
            if (actionService == null)
                throw new ArgumentNullException("actionService");
            if (batchService == null)
                throw new ArgumentNullException("batchService");
            if (userService == null)
                throw new ArgumentNullException("userService");

            _actionService = actionService;
            _batchService = batchService;
            _userService = userService;
        }

        //
        // GET: /BatchAction/

        public ActionResult Index(int batchId = 0)
        {
            IEnumerable<BatchAction> actions = _actionService.GetAllForBatch(batchId);
            return View(actions);
        }

        //
        // GET: /BatchAction/Details/5

        public ActionResult Details(int id = 0)
        {
            BatchAction batchaction = _actionService.Get(id);
            if (batchaction == null)
            {
                return HttpNotFound();
            }
            CheckViewAuthorization(id);

            //See if they can edit so we can disable things if this is read only
            //This removed all buttons from the view that require edit privs.
            int currentUserId = _userService.GetCurrentUserId();
            ViewBag.CanEdit = batchaction.CanEdit(currentUserId);

            return View(batchaction);
        }

        public ActionResult Create(int batchId = 0)
        {
            //No authorization needed here
            int userId = _userService.GetCurrentUserId();
            if (userId == 0)
                return new HttpUnauthorizedResult();

            Batch batch = _batchService.Get(batchId);
            if (batch == null)
                return new HttpStatusCodeResult(500);

            return View();
        }

        //
        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BatchAction created)
        {
            //not authorization needed here
            int userId = _userService.GetCurrentUserId();
            if (userId == 0)
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid)
            {
                Batch batch = _batchService.Get(created.BatchId);
                if (batch == null)
                    return new HttpStatusCodeResult(500);

                created.PerformerId = userId;
                created.ActionDate = DateTime.Now;

                _actionService.Create(created);

                return RedirectToAction("Details", "Batch", new { id = created.BatchId });
            }
            return View(created);
        }

        //
        // GET: /BatchAction/Edit/5

        public ActionResult Edit(int id = 0)
        {
            BatchAction batchAction = _actionService.Get(id);
            if (batchAction == null)
            {
                return HttpNotFound();
            }
            CheckEditAuthorization(id);
            return View(batchAction);
        }

        //
        // POST: /BatchAction/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BatchAction batchAction)
        {
            CheckEditAuthorization(batchAction.ActionId);
            if (ModelState.IsValid)
            {
                _actionService.Update(batchAction);
                return RedirectToAction("Details/" + batchAction.BatchId, "Batch");
            }
            return View(batchAction);
        }

        //
        // GET: /BatchAction/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BatchAction batchaction = _actionService.Get(id);
            if (batchaction == null)
            {
                return HttpNotFound();
            }
            CheckEditAuthorization(id);
            return View(batchaction);
        }

        //
        // POST: /BatchAction/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckEditAuthorization(id);
            BatchAction batchAction = _actionService.Get(id);
            _actionService.Delete(batchAction);
            return RedirectToAction("Details/" + batchAction.BatchId, "Batch");
        }

        private void CheckViewAuthorization(int actionId)
        {
            BatchAction action = _actionService.Get(actionId);
            int currentUser = _userService.GetCurrentUserId();

            if (!action.CanView(currentUser))
            {
                throw new UnauthorizedAccessException("Cannot view this data.");
            }
        }

        private void CheckEditAuthorization(int actionId)
        {
            BatchAction action = _actionService.Get(actionId);
            int currentUser = _userService.GetCurrentUserId();

            if (!action.CanEdit(currentUser))
            {
                throw new UnauthorizedAccessException("Cannot edit data you do not own.");
            }
        }
    }
}