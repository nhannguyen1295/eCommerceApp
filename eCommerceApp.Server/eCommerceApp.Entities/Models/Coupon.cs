using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceApp.Entities.Models
{
    public class Coupon
    {
        public Coupon()
        {
            InsertedAt = DateTime.UtcNow.ToLocalTime();
            UpdatedAt = DateTime.UtcNow.ToLocalTime();
        }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; } = true;
        public double Value { get; set; }
        public bool Multiple { get; set; } = false; // Coupon can use with another coupon ?
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Order> SalesOrders { get; set; }
    }
}