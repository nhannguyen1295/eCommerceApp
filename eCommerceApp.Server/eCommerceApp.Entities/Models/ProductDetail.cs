using System;

namespace eCommerceApp.Entities.Models
{
    public class ProductDetail
    {
        public ProductDetail(DateTime insertedAt, DateTime updatedAt)
        {
            InsertedAt = DateTime.UtcNow.ToLocalTime();
            UpdatedAt = DateTime.UtcNow.ToLocalTime();
        }

        public Guid Id { get; set; }
        public string Feature { get; set; }
        public string Description { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}