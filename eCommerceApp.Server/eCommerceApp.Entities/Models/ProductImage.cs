using System;

namespace eCommerceApp.Entities.Models
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public string Path { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}