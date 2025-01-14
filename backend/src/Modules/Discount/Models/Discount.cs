using System;
using StoreBackend.Common;

namespace StoreBackend.Models
{
    public class Discount : BaseEntity
    {
        public string Code { get; set; }
        public int Percentage { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}