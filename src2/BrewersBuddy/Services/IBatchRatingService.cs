using BrewersBuddy.Models;
using System.Collections.Generic;

namespace BrewersBuddy.Services
{
    public interface IBatchRatingService : ICRUDService<BatchRating>
    {
        IEnumerable<BatchRating> GetAllForBatch(int batchId);
    }
}
