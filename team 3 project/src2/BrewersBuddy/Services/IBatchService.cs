using BrewersBuddy.Models;
using System;
using System.Collections.Generic;

namespace BrewersBuddy.Services
{
    public interface IBatchService : ICRUDService<Batch>, IDisposable
    {
        void AddAction(Batch batch, BatchAction action);
        void AddNote(Batch batch, BatchNote note);
        void AddMeasurement(Batch batch, Measurement measurement);
        IEnumerable<Batch> GetAllForUser(int userId);
    }
}
