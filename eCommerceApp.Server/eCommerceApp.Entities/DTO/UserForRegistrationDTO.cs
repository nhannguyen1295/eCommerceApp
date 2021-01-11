using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Entities.DTO
{
    public class UserForRegistrationDTO : UserForManipulation
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}