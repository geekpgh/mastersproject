using BrewersBuddy.Models;
using System;
using System.Collections.Generic;

namespace BrewersBuddy.Services
{
    public interface IMeasurementService : ICRUDService<Measurement>, IDisposable
    {
        IEnumerable<Measurement> GetAllForBatch(int batchId);
    }
}
