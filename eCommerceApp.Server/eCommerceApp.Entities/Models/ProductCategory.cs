using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceApp.Entities.Models
{
    public class ProductCategory
    {
        public ProductCategory()
        {
            InsertedAt = DateTime.UtcNow.ToLocalTime();
            UpdatedAt = DateTime.UtcNow.ToLocalTime();
        }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}