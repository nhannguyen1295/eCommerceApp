using System;

namespace eCommerceApp.Entities.Models
{
    public class CategoryAttribute
    {
        public CategoryAttribute()
        {
            InsertedAt = DateTime.UtcNow.ToLocalTime();
            UpdatedAt = DateTime.UtcNow.ToLocalTime();
        }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid AttributeId { get; set; }
        public Attribute Attribute { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
    }
}