using System;

namespace eCommerceApp.Entities.DTO
{
    public class CategoryForCreationDTO : CategoryForManipulation
    {
        public Guid? ParentCategoryId { get; set; }
    }
}