using StoreBackend.Common;

namespace StoreBackend.Models
{
    public class BasketProduct : BaseEntity
    {
        public int BasketId { get; set; }
        public Basket Basket { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}