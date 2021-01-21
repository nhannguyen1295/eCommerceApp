using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Entities.DTO
{
    public abstract class ProductForManipulation
    {
        [Required(ErrorMessage = "Name is required field.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public double RegularPrice { get; set; }
        public double DiscountPrice { get; set; }
        public long Quantity { get; set; }
        public bool Taxable { get; set; } = true;
    }
}