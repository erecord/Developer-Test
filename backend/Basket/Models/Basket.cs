using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace StoreBackend.Models
{
    public class Basket
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public List<Product> Products { get; set; }

    }
}