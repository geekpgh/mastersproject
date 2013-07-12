using BrewersBuddy.Models;
using BrewersBuddy.Services;
using System;
using System.Web.Mvc;

namespace BrewersBuddy.Controllers
{
    public class BatchNoteController : Controller
    {
        private readonly IBatchService _batchService;
        private readonly IBatchNoteService _noteService;

        public BatchNoteController(
            IBatchService batchService,
            IBatchNoteService noteService)
        {
            if (batchService == null)
                throw new ArgumentNullException("batchService");
            if (noteService == null)
                throw new ArgumentNullException("noteService");

            _batchService = batchService;
            _noteService = noteService;
        }

        //
        // GET: /BatchNote/Details/5

        public ActionResult Details(int id = 0)
        {
            BatchNote batchnote = _noteService.Get(id);
            if (batchnote == null)
            {
                return HttpNotFound();
            }
            return View(batchnote);
        }

        //
        // GET: /BatchNote/Edit/5

        public ActionResult Edit(int id = 0)
        {
            BatchNote note = _noteService.Get(id);
            if (note == null)
            {
                return HttpNotFound();
            }

            return View(note);
        }


        //
        // POST: /Batch/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BatchNote note)
        {
            if (ModelState.IsValid)
            {
                _noteService.Update(note);
                return RedirectToAction("Details/" + note.BatchId, "Batch");
            }
            return View(note);
        }


        //
        // GET: /Batch/Delete/5
        public ActionResult Delete(int id = 0)
        {

            BatchNote note = _noteService.Get(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        //
        // POST: /Batch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteNoteConfirmed(int id = 0)
        {
            BatchNote note = _noteService.Get(id);
            _noteService.Delete(note);
            return RedirectToAction("Details/" + note.BatchId, "Batch");
        }
    }
}