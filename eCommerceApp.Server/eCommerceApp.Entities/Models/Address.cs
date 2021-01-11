using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eCommerceApp.Entities.Models.Identity;

namespace eCommerceApp.Entities.Models
{
    public class Address
    {
        public Address()
        {
            InsertedAt = DateTime.UtcNow.ToLocalTime();
            UpdatedAt = DateTime.UtcNow.ToLocalTime();
        }
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } // Home, company's address ...
        [MaxLength(100)]
        public string NumberAndStreetName { get; set; } // Street name
        [MaxLength(50)]
        public string Ward { get; set; }
        [MaxLength(50)]
        public string District { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
#nullable enable
        [MaxLength(11)]
        public string? PhoneNumber { get; set; }

#nullable disable
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Order> SalesOrders { get; set; }
    }
}