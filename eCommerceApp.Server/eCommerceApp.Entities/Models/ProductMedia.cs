using System;

namespace eCommerceApp.Entities.Models
{
    public class ProductMedia
    {
        public ProductMedia()
        {
            InsertedAt = DateTime.UtcNow.ToLocalTime();
            UpdatedAt = DateTime.UtcNow.ToLocalTime();
        }
        public Guid Id { get; set; }
        public MediaType Type { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }

    public enum MediaType
    {
        Video = 1,
        Picture
    }
}