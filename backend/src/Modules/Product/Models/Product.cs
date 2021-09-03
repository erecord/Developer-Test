using StoreBackend.Common;

namespace StoreBackend.Models
{
    public class Product : BaseEntity
    {

        public string Name { get; set; }
        public float Price { get; set; }

    }
}