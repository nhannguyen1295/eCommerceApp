using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Entities.Models
{
    public class ProductAttributeValue
    {
        public ProductAttributeValue()
        {
            UpdatedAt = InsertedAt = DateTime.UtcNow.ToLocalTime();
        }
        [Key]
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public ICollection<AttributeValue> AttributeValues { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}