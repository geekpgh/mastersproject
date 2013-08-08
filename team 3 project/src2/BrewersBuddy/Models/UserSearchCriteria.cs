using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrewersBuddy.Models
{
    public class UserSearchCriteria
    {
        public UserSearchCriteria()
        {
            UserName = String.Empty;
            FirstName = String.Empty;
            LastName = String.Empty;
            Zipcode = String.Empty;
        }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Zipcode { get; set; }
    }
}