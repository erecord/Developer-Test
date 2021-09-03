using System.Collections.Generic;
using StoreBackend.Models;

namespace StoreBackend.DTOs
{
    public record UpdateBasketDTO
    {
        public Basket Basket { get; init; }
        public List<int> ProductIds { get; init; }
    }
}