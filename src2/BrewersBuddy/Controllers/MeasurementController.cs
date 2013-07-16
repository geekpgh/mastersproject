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
        // GET: /Measurment/

        public ActionResult Index(int batchId)
        {
            IEnumerable<Measurement> measurements = _measurementService.GetAllForBatch(batchId);
            return View(measurements);
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
        // POST: /BatchRating/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Measurement measurement)
        {
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
            Measurement measurement = _measurementService.Get(id);
            if (measurement == null)
            {
                return HttpNotFound();
            }
            return View(measurement);
        }

        //
        // GET: /Measurment/Edit/5

        public ActionResult Edit(int id = 0)
        {
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
            Measurement measurement = _measurementService.Get(id);
            _measurementService.Delete(measurement);
            return RedirectToAction("Details/" + measurement.BatchId, "Batch");
        }
    }
}