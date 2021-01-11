using System;

namespace eCommerceApp.Entities.DTO
{
    public class CategoryForUpdateDTO : CategoryForManipulation
    {
        public Guid? ParentCategoryId { get; set; }
    }
}