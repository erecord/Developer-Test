using System;
using StoreBackend.Models;

namespace StoreBackend.Tests.Factories
{
    public static class TestUserFactory
    {
        public static User CreateRandomUser(int userId) => new User
        {
            Id = userId,
            Email = Guid.NewGuid().ToString(),
            Username = Guid.NewGuid().ToString(),
            PasswordHashed = Guid.NewGuid().ToString()
        };
    }
}