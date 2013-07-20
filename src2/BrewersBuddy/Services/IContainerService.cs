using BrewersBuddy.Models;
using System;
using System.Collections.Generic;

namespace BrewersBuddy.Services
{
    public interface IContainerService : ICRUDService<Container>, IDisposable
    {
        IEnumerable<Container> GetAllForUser(int userId);
    }
}
