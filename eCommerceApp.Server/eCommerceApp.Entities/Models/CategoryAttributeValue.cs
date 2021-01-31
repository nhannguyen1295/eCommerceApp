using System;

namespace eCommerceApp.Entities.Models
{
    public class CategoryAttributeValue
    {
        public CategoryAttributeValue()
        {
            InsertedAt = DateTime.UtcNow.ToLocalTime();
            UpdatedAt = DateTime.UtcNow.ToLocalTime();
        }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid AttributeValueId { get; set; }
        public AttributeValue AttributeValue { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}