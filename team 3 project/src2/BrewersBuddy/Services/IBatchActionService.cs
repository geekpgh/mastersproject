using BrewersBuddy.Models;
using System;
using System.Collections.Generic;

namespace BrewersBuddy.Services
{
    public interface IBatchActionService : ICRUDService<BatchAction>, IDisposable
    {
        IEnumerable<BatchAction> GetAllForBatch(int batchId);
    }
}
