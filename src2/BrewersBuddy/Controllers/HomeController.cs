using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrewersBuddy.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
			if (string.IsNullOrWhiteSpace(User.Identity.Name))
			{
				TempData["ShowButton"] = true;
			}
			else
			{
				TempData["ShowButton"] = false;
			}
            return View();
        }
    }
}
