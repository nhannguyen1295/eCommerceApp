using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Entities.Models
{
    public class AttributeValue
    {
        public AttributeValue()
        {
            InsertedAt = UpdatedAt = DateTime.UtcNow.ToLocalTime();
        }

        public string Value { get; set; }
        public string Description { get; set; }

        public Guid AttributeId { get; set; }
        public Attribute Attribute { get; set; }

        public Guid ProductAttributeValueId { get; set; }
        public ProductAttributeValue ProductAttributeValue { get; set; }

        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}