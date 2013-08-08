using System;
using System.Data.Entity;
using BrewersBuddy.Models;
using WebMatrix.WebData;

namespace BrewersBuddy.Tests
{
    class DatabaseInitializer : DropCreateDatabaseAlways<BrewersBuddyContext>
    {
        protected override void Seed(BrewersBuddyContext context)
        {
            base.Seed(context);
        }
    }
}
