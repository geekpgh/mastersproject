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

        public BatchActionController(IBatchActionService actionService)
        {
            if (actionService == null)
                throw new ArgumentNullException("actionService");

            _actionService = actionService;
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

        //
        // GET: /BatchAction/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BatchAction/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BatchAction batchAction)
        {
            if (ModelState.IsValid)
            {
                batchAction.PerformerId = ControllerUtils.GetCurrentUserId(User);
                _actionService.Create(batchAction);
                return RedirectToAction("Index");
            }

            return View(batchAction);
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
                return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }
    }
}