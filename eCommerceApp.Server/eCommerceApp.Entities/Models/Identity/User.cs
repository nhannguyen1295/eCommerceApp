using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace eCommerceApp.Entities.Models.Identity
{
    public class User : IdentityUser
    {
        public User()
        {
            InsertedAt = DateTime.UtcNow.ToLocalTime();
            UpdatedAt = DateTime.UtcNow.ToLocalTime();
            IsActive = true;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Order> SalesOrders { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<PaymentMethod> PaymentMethods { get; set; }

        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}