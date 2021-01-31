using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceApp.Entities.Models
{
    public class Category
    {
        public Category()
        {
            InsertedAt = DateTime.UtcNow.ToLocalTime();
            UpdatedAt = DateTime.UtcNow.ToLocalTime();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }

        // 1- n self-referencing
        public Guid? ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; }
        public ICollection<Category> ParentCategories { get; set; }

        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // 1 - n referencing to ProductCategory
        public ICollection<ProductCategory> ProductCategories { get; set; }

        public ICollection<CategoryAttributeValue> CategoryAttributeValues { get; set; }
    }
}