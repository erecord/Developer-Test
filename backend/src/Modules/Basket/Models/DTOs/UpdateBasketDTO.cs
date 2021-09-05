using System.Collections.Generic;
using StoreBackend.Models;

namespace StoreBackend.DTOs
{
    public record UpdateBasketDTO
    {
        public Basket Basket { get; init; }
        public IEnumerable<int> ProductIds { get; init; }
    }
}