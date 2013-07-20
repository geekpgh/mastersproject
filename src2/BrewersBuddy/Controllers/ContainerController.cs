using BrewersBuddy.Filters;
using BrewersBuddy.Models;
using BrewersBuddy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BrewersBuddy.Controllers
{
    [Authorize]
    public class ContainerController : Controller
    {
        private readonly IBatchService _batchService;
        private readonly IContainerService _containerService;
        private readonly IUserService _userService;

        public ContainerController(
            IBatchService batchService,
            IContainerService containerService,
            IUserService userService)
        {
            if (batchService == null)
                throw new ArgumentNullException("batchService");
            if (containerService == null)
                throw new ArgumentNullException("containerService");
            if (userService == null)
                throw new ArgumentNullException("userService");

            _batchService = batchService;
            _containerService = containerService;
            _userService = userService;
        }

        //
        // GET: /Container/

        public ActionResult Index()
        {
            int currentUserId = _userService.GetCurrentUserId();
            IEnumerable<Container> containers = _containerService.GetAllForUser(currentUserId);
            return View(containers);
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
        // POST: /Container/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Container container)
        {
            int userId = _userService.GetCurrentUserId();

            if (userId == 0)
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid)
            {
                Batch batch = _batchService.Get(container.BatchId);

                if (batch == null)
                    return new HttpStatusCodeResult(500);

                container.OwnerId = userId;

                _containerService.Create(container);

                return RedirectToAction("Details", "Batch", new { id = container.BatchId });
            }
            return View(container);
        }

        //
        // GET: /Container/Details
        public ActionResult Details(int id = 0)
        {
            Container container = _containerService.Get(id);
            if (container == null)
            {
                return HttpNotFound();
            }
            return View(container);
        }


        //
        // GET: /Container/Edit
        public ActionResult Edit(int id = 0)
        {
            CheckAuthorization(id);
            Container container = _containerService.Get(id);

            if (container == null)
            {
                return HttpNotFound();
            }
            return View(container);
        }


        //
        // POST: /Container/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Container container)
        {
            CheckAuthorization(container.ContainerId);

            if (ModelState.IsValid)
            {
                _containerService.Update(container);
                return RedirectToAction("Details/" + container.ContainerId);
            }
            return View(container);
        }


        //
        // GET: /Container/Delete
        public ActionResult Delete(int id = 0)
        {
            CheckAuthorization(id);
            Container container = _containerService.Get(id);
            if (container == null)
            {
                return HttpNotFound();
            }
            return View(container);
        }

        //
        // POST: /Container/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckAuthorization(id);
            Container container = _containerService.Get(id);
            _containerService.Delete(container);
            return RedirectToAction("Index");
        }


        private void CheckAuthorization(int containerId)
        {
            int currentUser = _userService.GetCurrentUserId();
            int containerOwner = _containerService.Get(containerId).OwnerId;

            if (containerOwner != currentUser)
            {
                throw new UnauthorizedAccessException("Cannot alter data you do not own.");
            }
        }



    }
}
