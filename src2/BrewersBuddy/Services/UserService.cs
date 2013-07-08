using System.Security.Principal;
using System.Web;
using WebMatrix.WebData;

namespace BrewersBuddy.Services
{
    public class UserService : IUserService
    {
        public IPrincipal GetCurrentUser()
        {
            return HttpContext.Current.User;
        }

        public int GetCurrentUserId()
        {
            IPrincipal currentUser = GetCurrentUser();

            if (currentUser == null)
                return 0;

            return WebSecurity.GetUserId(currentUser.Identity.Name);
        }
    }
}