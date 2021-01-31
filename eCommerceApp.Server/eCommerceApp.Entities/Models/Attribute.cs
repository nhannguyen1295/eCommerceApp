using System;
using System.Collections.Generic;

namespace eCommerceApp.Entities.Models
{
    public class Attribute
    {
        public Attribute()
        {
            InsertedAt = UpdatedAt = DateTime.UtcNow.ToLocalTime();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
#nullable enable
        public string? Description { get; set; }
#nullable disable
        public ICollection<AttributeValue> AttributeValues { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}