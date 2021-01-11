using System;
using eCommerceApp.Entities.Models;

namespace eCommerceApp.Entities.DTO
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime InsertedAt { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }
}