using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BrewersBuddy.Controllers
{
    [Authorize]
    public class BatchRatingController : Controller
    {
        private BrewersBuddyContext db = new BrewersBuddyContext();

        //
        // GET: /BatchRating/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /BatchRating/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /BatchRating/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BatchRating/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /BatchRating/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /BatchRating/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /BatchRating/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /BatchRating/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
