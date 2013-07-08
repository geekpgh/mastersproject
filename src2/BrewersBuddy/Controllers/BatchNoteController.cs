using BrewersBuddy.Models;
using BrewersBuddy.Services;
using System;
using System.Web.Mvc;

namespace BrewersBuddy.Controllers
{
    public class BatchNoteController : Controller
    {
        private readonly IBatchNoteService _noteService;

        public BatchNoteController(IBatchNoteService noteService)
        {
            if (noteService == null)
                throw new ArgumentNullException("noteService");

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
            BatchNote batchnote = _noteService.Get(id);
            if (batchnote == null)
            {
                return HttpNotFound();
            }
            return View(batchnote);
        }

        //
        // POST: /BatchNote/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BatchNote batchNote)
        {
            if (ModelState.IsValid)
            {
                _noteService.Update(batchNote);
                return RedirectToAction("Index");
            }
            return View(batchNote);
        }
    }
}