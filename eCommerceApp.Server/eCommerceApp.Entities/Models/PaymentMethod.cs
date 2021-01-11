using System;
using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Entities.Models
{
    public class PaymentMethod
    {
        public Guid Id { get; set; }
        [MaxLength(16)]
        public string CCNumber { get; set; }
        // Expiration date MM/YY
        [MaxLength(2)]
        public string MM { get; set; }
        [MaxLength(2)]
        public string YY { get; set; }
    }
}