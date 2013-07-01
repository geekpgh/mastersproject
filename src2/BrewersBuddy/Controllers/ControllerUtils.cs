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
        public static IEnumerable<SelectListItem> getSelectionForEnum<T>()
        {
            try
            {
                return Enum.GetValues(typeof(T))
                    .Cast<T>()
                    .Select(x => new SelectListItem()
                    {
                        Text = x.ToString(),
                        Value = Convert.ToInt32(x).ToString()
                    });
            }
            catch
            {
                return new List<SelectListItem>();
            }
        }

        public static int getCurrentUserId(IPrincipal user)
        {
            return WebSecurity.GetUserId(user.Identity.Name);
        }
    }
}