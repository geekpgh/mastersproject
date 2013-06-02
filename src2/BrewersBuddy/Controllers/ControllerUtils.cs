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
        public static List<SelectListItem> getSelectionForEnum<T>()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var values = Enum.GetValues(typeof(T));

            int valueCount = 0;
            foreach (var value in values)
            {
                items.Add(new SelectListItem { Text = value.ToString(), Value = valueCount.ToString() });
            }

            return items;
        }

        public static int getCurrentUserId(IPrincipal user)
        {
            return WebSecurity.GetUserId(user.Identity.Name);
        }
    }
}