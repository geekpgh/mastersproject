using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrewersBuddy.Services;
using System.Security.Principal;

namespace BrewersBuddy.Controllers
{
    public class HomeController : Controller
    {
        private IUserService _userService;

        public HomeController(IUserService userService)
        {
            if (userService == null)
                throw new ArgumentNullException("userService");

            _userService = userService;
        }

        public ActionResult Index()
        {
            IPrincipal currentUser = _userService.GetCurrentUser();

			if (currentUser != null)
			{
                return RedirectToAction("Index", "Batch");
			}

            return View();
        }
    }
}
