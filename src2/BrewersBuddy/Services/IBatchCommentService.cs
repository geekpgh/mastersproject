using BrewersBuddy.Models;
using System;
using System.Collections.Generic;

namespace BrewersBuddy.Services
{
    public interface IBatchCommentService : ICRUDService<BatchComment>, IDisposable
    {
        IEnumerable<BatchComment> GetAllForBatch(int batchId);
    }
}
