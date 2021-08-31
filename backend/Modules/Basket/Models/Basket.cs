using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreBackend.Models
{
    public class Basket
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [NotMapped]
        public List<Product> Products { get; set; }

    }
}