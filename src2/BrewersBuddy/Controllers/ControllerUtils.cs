using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace BrewersBuddy.Controllers
{
    public class ControllerUtils
    {

        public static int GetCurrentUserId(IPrincipal user)
        {
            return WebSecurity.GetUserId(user.Identity.Name);
        }
    }
}