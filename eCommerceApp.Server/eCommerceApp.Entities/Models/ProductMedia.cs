using System;

namespace eCommerceApp.Entities.Models
{
    public class ProductMedia
    {
        public ProductMedia()
        {
            InsertedAt = DateTime.UtcNow.ToLocalTime();
            UpdatedAt = DateTime.UtcNow.ToLocalTime();
        }
        public Guid Id { get; set; }
        public string Path { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}