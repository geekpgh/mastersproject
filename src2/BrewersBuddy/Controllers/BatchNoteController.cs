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
        private readonly IUserService _userService;

        public BatchNoteController(
            IBatchService batchService,
            IBatchNoteService noteService,
            IUserService userService)
        {
            if (batchService == null)
                throw new ArgumentNullException("batchService");
            if (noteService == null)
                throw new ArgumentNullException("noteService");
            if (userService == null)
                throw new ArgumentNullException("userService");

            _batchService = batchService;
            _noteService = noteService;
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

            return View();
        }

        //
        // POST: /BatchRating/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BatchNote note)
        {
            int userId = _userService.GetCurrentUserId();
            if (userId == 0)
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid)
            {
                Batch batch = _batchService.Get(note.BatchId);
                if (batch == null)
                    return new HttpStatusCodeResult(500);

                note.AuthorId = userId;
                note.AuthorDate = DateTime.Now;

                _noteService.Create(note);

                return RedirectToAction("Details", "Batch", new { id = note.BatchId });
            }

            return View(note);
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