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
            return View(batchaction);
        }

        public ActionResult Create(int batchId = 0)
        {
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
            return View(batchAction);
        }

        //
        // POST: /BatchAction/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BatchAction batchAction)
        {
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
            return View(batchaction);
        }

        //
        // POST: /BatchAction/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BatchAction batchAction = _actionService.Get(id);
            _actionService.Delete(batchAction);
            return RedirectToAction("Details/" + batchAction.BatchId, "Batch");
        }
    }
}