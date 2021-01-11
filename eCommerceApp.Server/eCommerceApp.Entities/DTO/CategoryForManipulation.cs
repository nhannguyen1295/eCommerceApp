using System;
using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Entities.DTO
{
    public class CategoryForManipulation
    {
        [Required(ErrorMessage = "Category name is required field.")]
        [MaxLength(255, ErrorMessage = "Maximum length for the Name is 255 characters.")]
        public string Name { get; set; }
    }
}