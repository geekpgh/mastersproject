using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrewersBuddy;
using BrewersBuddy.Controllers;
using BrewersBuddy.Filters;
using BrewersBuddy.Models;

namespace BrewersBuddy.Models
{
    public class RecipeController : Controller
    {
        //
        // GET: /Default1/

        public ActionResult Index()
        {
            return View();
        }

    }
}
