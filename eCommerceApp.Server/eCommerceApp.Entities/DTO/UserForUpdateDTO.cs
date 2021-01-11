using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Entities.DTO
{
    public class UserForUpdateDTO : UserForManipulation
    {
        public string Password { get; set; }
    }
}