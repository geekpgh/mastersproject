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

        public MeasurementController(IMeasurementService measurementService)
        {
            if (measurementService == null)
                throw new ArgumentNullException("measurementService");

            _measurementService = measurementService;
        }

        //
        // GET: /Measurment/

        public ActionResult Index(int batchId)
        {
            IEnumerable<Measurement> measurements = _measurementService.GetAllForBatch(batchId);
            return View(measurements);
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