using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreBackend.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public int discountId { get; set; }

        [NotMapped]
        public List<Product> Products { get; set; }

        public User User { get; set; }
        public Discount Discount { get; set; }

    }
}