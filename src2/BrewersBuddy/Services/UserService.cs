using BrewersBuddy.Models;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using WebMatrix.WebData;
using System.Linq;

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
            ICollection<UserProfile> friendProfiles = new List<UserProfile>();
            foreach (Friend friend in friends)
            {
                //Don't double add or include the user themself
                if (!friendProfiles.Contains(friend.User) && friend.UserId != id)
                {
                    friendProfiles.Add(friend.User);
                }

                //Don't double add or include the user themself
                if (!friendProfiles.Contains(friend.FriendUser) && friend.FriendUserId != id)
                {
                    friendProfiles.Add(friend.FriendUser);
                }

            }

            return friendProfiles;
        }

        public void Dispose()
        {
            if (db != null)
                db.Dispose();
        }
    }
}