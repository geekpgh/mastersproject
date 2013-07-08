using BrewersBuddy.Models;
using System;
using System.Collections.Generic;

namespace BrewersBuddy.Services
{
    public interface IBatchNoteService : ICRUDService<BatchNote>, IDisposable
    {
        IEnumerable<BatchNote> GetAllForBatch(int batchId);
    }
}
