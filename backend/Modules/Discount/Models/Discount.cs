using System;

namespace StoreBackend.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Percentage { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}