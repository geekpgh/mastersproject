using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrewersBuddy.Controllers
{
    [Authorize]
    public class ContainerController : Controller
    {
        //
        // GET: /Container/

        public ActionResult Index()
        {
            return View();
        }

    }
}
