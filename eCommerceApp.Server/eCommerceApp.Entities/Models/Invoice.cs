using System;
using System.Collections.Generic;

namespace eCommerceApp.Entities.Models
{
    public class Invoice
    {
        public Invoice()
        {
            InvoiceDate = DateTime.UtcNow.ToLocalTime();
            InvoiceStatusCode = InvoiceStatusCode.Waiting;
        }
        public Guid Id { get; set; }
        public DateTime InvoiceDate { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public ICollection<Shipment> Shipments { get; set; }
        public double TotalPrice { get; set; }
        public InvoiceStatusCode InvoiceStatusCode { get; set; }
    }

    public enum InvoiceStatusCode
    {
        Issued = -1,
        Waiting = 0,
        Paid = 1
    }
}