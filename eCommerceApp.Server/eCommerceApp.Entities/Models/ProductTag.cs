using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceApp.Entities.Models
{
    public class ProductTag
    {
        public ProductTag()
        {
            InsertedAt = DateTime.UtcNow.ToLocalTime();
            UpdatedAt = DateTime.UtcNow.ToLocalTime();
        }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}