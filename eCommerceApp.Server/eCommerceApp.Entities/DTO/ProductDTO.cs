using System;

namespace eCommerceApp.Entities.DTO
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double RegularPrice { get; set; }
        public double DiscountPrice { get; set; }
        public long Quantity { get; set; }
        public bool Taxable { get; set; } = true;
    }
}