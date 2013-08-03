using BrewersBuddy.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebMatrix.WebData;

namespace BrewersBuddy.Services
{
    public class UserService : IUserService
    {
        private BrewersBuddyContext db = new BrewersBuddyContext();

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

        public IEnumerable<UserProfile> Find(UserSearchCriteria searchCriteria)
        {
            int currentUserId = GetCurrentUserId();

            return from user in db.UserProfiles
                   where
                       (user.UserName.Equals(searchCriteria.UserName, System.StringComparison.OrdinalIgnoreCase)
                       || user.FirstName.Equals(searchCriteria.FirstName, System.StringComparison.OrdinalIgnoreCase)
                       || user.LastName.Equals(searchCriteria.LastName, System.StringComparison.OrdinalIgnoreCase)
                       || user.Zip.Equals(searchCriteria.Zipcode, System.StringComparison.OrdinalIgnoreCase))
                       && user.UserId != currentUserId
                   select user;
        }

        public UserProfile Get(int id)
        {
            return db.UserProfiles.Find(id);
        }

        public ICollection<Friend> Friends(int id)
        {
            UserProfile user = Get(id);

            return user.Friends;
        }

        public ICollection<UserProfile> FriendProfiles(int id)
        {
            ICollection<Friend> friends = Friends(id);

            return friends.Select(friend => friend.FriendUser)
                .ToList();
        }

        public void Dispose()
        {
            if (db != null)
                db.Dispose();
        }
    }
}