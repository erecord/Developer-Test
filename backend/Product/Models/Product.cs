using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StoreBackend.Models
{
    public class Product
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }

    }
}