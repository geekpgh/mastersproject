using BrewersBuddy.Models;
using System;
using System.Collections.Generic;

namespace BrewersBuddy.Services
{
    public interface IRecipeService : ICRUDService<Recipe>, IDisposable
    {
        IEnumerable<Recipe> GetAllForUser(int userId);
    }
}
