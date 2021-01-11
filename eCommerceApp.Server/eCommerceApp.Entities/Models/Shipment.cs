using System;
using System.Collections.Generic;

namespace eCommerceApp.Entities.Models
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public string ShimentTrackingNumber { get; set; }
        public DateTime ShipmentDate { get; set; }

        public ICollection<ShipmentOrderItem> ShipmentOrderItems { get; set; }
    }
}