using System;

namespace StoreBackend.Exceptions
{
    public class DiscountCodeNotFoundException : Exception
    {
        public DiscountCodeNotFoundException(string discountCode) : base($"The discount with code '{discountCode}' was not found")
        {
        }
    }
}