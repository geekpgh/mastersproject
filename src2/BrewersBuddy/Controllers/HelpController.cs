using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrewersBuddy.Controllers
{
    public class HelpController : Controller
    {
        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult Manual()
        {
            return View();
        }

    }
}
