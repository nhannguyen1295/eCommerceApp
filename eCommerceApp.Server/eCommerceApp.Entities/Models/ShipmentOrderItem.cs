using System;

namespace eCommerceApp.Entities.Models
{
    public class ShipmentOrderItem
    {
        public Guid OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; }

        public Guid ShipmentId { get; set; }
        public Shipment Shipment { get; set; }
    }
}