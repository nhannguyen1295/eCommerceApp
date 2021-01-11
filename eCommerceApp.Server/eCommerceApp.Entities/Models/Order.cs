using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eCommerceApp.Entities.Models.Identity;

namespace eCommerceApp.Entities.Models
{
    public class Order
    {
        public Order()
        {
            InsertedAt = DateTime.UtcNow.ToLocalTime();
            UpdatedAt = DateTime.UtcNow.ToLocalTime();
        }

        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public double Total { get; set; }

        public Guid CouponId { get; set; }
        public Coupon Coupon { get; set; }

        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public Guid AddressId { get; set; }
        public Address Address { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Waiting;

        public ICollection<Invoice> Invoices { get; set; }
    }

    public enum OrderStatus
    {
        Cancelled = -1,
        Waiting = 0,
        Completed = 1
    }
}