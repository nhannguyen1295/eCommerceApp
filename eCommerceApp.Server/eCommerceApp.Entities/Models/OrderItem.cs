using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceApp.Entities.Models
{
    public class OrderItem
    {
        public OrderItem()
        {
            InsertedAt = DateTime.UtcNow.ToLocalTime();
            UpdatedAt = DateTime.UtcNow.ToLocalTime();
        }
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Subtotal { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public OrderItemStatus OrderItemStatus { get; set; } = OrderItemStatus.InStock;

        public ICollection<ShipmentOrderItem> ShipmentOrderItems { get; set; }
    }

    public enum OrderItemStatus
    {
        OutOfStock = -1,
        InStock = 0,
        Delivered = 1
    }
}