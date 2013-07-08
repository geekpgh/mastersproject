using BrewersBuddy.Models;
using System;
using System.Collections.Generic;

namespace BrewersBuddy.Services
{
    public interface IBatchRatingService : ICRUDService<BatchRating>, IDisposable
    {
        IEnumerable<BatchRating> GetAllForBatch(int batchId);
    }
}
