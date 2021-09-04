using System;
using Microsoft.EntityFrameworkCore;
using StoreBackend.DbContexts;

namespace StoreBackend.Tests
{
    public class BaseTest
    {
        public StoreDbContext GetDbContext()
        {

            var builder = new DbContextOptionsBuilder<StoreDbContext>()
             .EnableSensitiveDataLogging()
             .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var dbContext = new StoreDbContext(builder.Options);
            dbContext.Database.EnsureCreated();

            return dbContext;
        }
    }
}