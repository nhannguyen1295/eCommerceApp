using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceApp.Entities.Models
{
    public class Product
    {
        public Product()
        {
            InsertedAt = DateTime.UtcNow.ToLocalTime();
            UpdatedAt = DateTime.UtcNow.ToLocalTime();
            ProductStatus = Quantity > 0 ? ProductStatus.Availabel : ProductStatus.NonAvailabel;
        }
        public Guid Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }

        // n - 1 referencing to ProductStatus 
        public Guid ProductStatusId { get; set; }

        public double RegularPrice { get; set; }
        public double DiscountPrice { get; set; }
        public long Quantity { get; set; }
        public bool Taxable { get; set; } = false;
        public ProductStatus ProductStatus { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // 1 - n referencing to ProductCategory
        public ICollection<ProductCategory> ProductCategories { get; set; }

        public ICollection<ProductTag> ProductTags { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductVideo> ProductVideos { get; set; }

        public ICollection<ProductDetail> ProductDetails { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public ICollection<Invoice> Invoices { get; set; }
    }

    public enum ProductStatus
    {
        NonAvailabel = -1,
        Availabel = 0,
        Promotion = 1
    }
}