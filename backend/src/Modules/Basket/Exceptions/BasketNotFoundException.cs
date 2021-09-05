using System;

namespace StoreBackend.Exceptions
{
    public class BasketNotFoundException : Exception
    {
        public BasketNotFoundException(int basketId) : base($"The basket with id {basketId} was not found")
        {
        }
    }
}