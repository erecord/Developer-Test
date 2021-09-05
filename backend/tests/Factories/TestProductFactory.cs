using System;
using StoreBackend.Models;

namespace StoreBackend.Tests.Factories
{
    public static class TestProductFactory
    {
        public static Product CreateProduct(int id, decimal price) => new Product
        {
            Id = id,
            Name = Guid.NewGuid().ToString(),
            Price = price
        };
    }
}