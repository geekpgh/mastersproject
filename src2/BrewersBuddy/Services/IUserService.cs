using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace BrewersBuddy.Services
{
    public interface IUserService : IDisposable
    {
        IPrincipal GetCurrentUser();
        int GetCurrentUserId();
        UserProfile Get(int id);
        IEnumerable<UserProfile> Find(UserSearchCriteria searchCriteria);
        ICollection<Friend> Friends(int id);
        ICollection<UserProfile> FriendProfiles(int id);
    }
}
