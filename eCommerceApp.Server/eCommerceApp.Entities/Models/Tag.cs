using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceApp.Entities.Models
{
    public class Tag
    {
        public Tag()
        {
            InsertedAt = DateTime.UtcNow.ToLocalTime();
            UpdatedAt = DateTime.UtcNow.ToLocalTime();
        }
        public Guid Id { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<ProductTag> ProductTags { get; set; }
    }
}