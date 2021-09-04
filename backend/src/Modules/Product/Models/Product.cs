using System.ComponentModel.DataAnnotations.Schema;
using StoreBackend.Common;

namespace StoreBackend.Models
{
    public class Product : BaseEntity
    {

        public string Name { get; set; }

        [Column(TypeName = "decimal(16,3)")]
        public decimal Price { get; set; }

    }
}