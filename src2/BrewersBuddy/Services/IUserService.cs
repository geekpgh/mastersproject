using System.Security.Principal;

namespace BrewersBuddy.Services
{
    public interface IUserService
    {
        IPrincipal GetCurrentUser();
        int GetCurrentUserId();
    }
}
