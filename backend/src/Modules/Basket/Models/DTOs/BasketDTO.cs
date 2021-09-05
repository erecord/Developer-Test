using System.Collections.Generic;
using StoreBackend.Models;

namespace StoreBackend.DTOs
{
    public record BasketDTO
    {

        public int Id { get; init; }
        public int userId { get; init; }
        public int? discountId { get; init; }

        public List<Product> Products { get; set; }

        public User User { get; init; }
        public Discount Discount { get; init; }
    }
}