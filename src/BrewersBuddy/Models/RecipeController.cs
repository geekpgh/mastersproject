using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrewersBuddy;
using BrewersBuddy.Controllers;

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
