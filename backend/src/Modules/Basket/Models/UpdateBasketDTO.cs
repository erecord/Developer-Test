using System.Collections.Generic;

namespace StoreBackend.Models
{
    public class UpdateBasketDTO
    {
        public Basket Basket { get; set; }
        public List<int> ProductIds { get; set; }
    }
}