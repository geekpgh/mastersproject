using BrewersBuddy.Models;
using BrewersBuddy.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BrewersBuddy.Controllers
{
    [Authorize]
    public class MeasurementController : Controller
    {
        private readonly IMeasurementService _measurementService;
        private readonly IUserService _userService;
        private readonly IBatchService _batchService;

        public MeasurementController(IMeasurementService measurementService,
                                     IUserService userService,
                                     IBatchService batchService)
        {
            if (measurementService == null)
                throw new ArgumentNullException("measurementService");
            if (userService == null)
                throw new ArgumentNullException("userService");
            if (batchService == null)
                throw new ArgumentNullException("batchService");

            _measurementService = measurementService;
            _userService = userService;
            _batchService = batchService;
        }

        //
        // GET: /Measurement/

        public ActionResult Index(int batchId)
        {
            IEnumerable<Measurement> measurements = _measurementService.GetAllForBatch(batchId);
            return View(measurements);
        }

        public ActionResult Create(int batchId = 0)
        {
            //No Authorization needed here
            int userId = _userService.GetCurrentUserId();
            if (userId == 0)
                return new HttpUnauthorizedResult();

            Batch batch = _batchService.Get(batchId);
            if (batch == null)
                return new HttpStatusCodeResult(500);

            return View();
        }

        //
        // POST: /BatchRating/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Measurement measurement)
        {
            //No Authorization needed here
            int userId = _userService.GetCurrentUserId();
            if (userId == 0)
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid)
            {
                Batch batch = _batchService.Get(measurement.BatchId);
                if (batch == null)
                    return new HttpStatusCodeResult(500);

                measurement.MeasurementDate = DateTime.Now;

                _measurementService.Create(measurement);

                return RedirectToAction("Details", "Batch", new { id = measurement.BatchId });
            }

            return View(measurement);
        }

        //
        // GET: /Measurment/Details/5

        public ActionResult Details(int id = 0)
        {
            CheckViewAuthorization(id);
            Measurement measurement = _measurementService.Get(id);
            if (measurement == null)
            {
                return HttpNotFound();
            }

            //See if they can edit so we can disable things if this is read only
            //This removed all buttons from the view that require edit privs.
            int currentUserId = _userService.GetCurrentUserId();
            ViewBag.CanEdit = measurement.CanEdit(currentUserId);

            return View(measurement);
        }

        //
        // GET: /Measurment/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CheckEditAuthorization(id);
            Measurement measurement = _measurementService.Get(id);
            if (measurement == null)
            {
                return HttpNotFound();
            }
            return View(measurement);
        }

        //
        // POST: /Measurment/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Measurement measurement)
        {
            CheckEditAuthorization(measurement.MeasurementId);
            if (ModelState.IsValid)
            {
                _measurementService.Update(measurement);
                return RedirectToAction("Details/" + measurement.BatchId, "Batch");
            }
            return View(measurement);
        }

        //
        // GET: /Measurment/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CheckEditAuthorization(id);
            Measurement measurement = _measurementService.Get(id);
            if (measurement == null)
            {
                return HttpNotFound();
            }
            return View(measurement);
        }

        //
        // POST: /Measurment/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckEditAuthorization(id);
            Measurement measurement = _measurementService.Get(id);
            _measurementService.Delete(measurement);
            return RedirectToAction("Details/" + measurement.BatchId, "Batch");
        }

        private void CheckViewAuthorization(int measurementId)
        {
            Measurement measurement = _measurementService.Get(measurementId);
            int currentUser = _userService.GetCurrentUserId();

            if (!measurement.CanView(currentUser))
            {
                throw new UnauthorizedAccessException("Cannot view this data.");
            }
        }

        private void CheckEditAuthorization(int measurementId)
        {
            Measurement measurement = _measurementService.Get(measurementId);
            int currentUser = _userService.GetCurrentUserId();

            if (!measurement.CanEdit(currentUser))
            {
                throw new UnauthorizedAccessException("Cannot edit data you do not own.");
            }
        }
    }
}