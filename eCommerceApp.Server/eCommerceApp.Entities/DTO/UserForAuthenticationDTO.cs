using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Entities.DTO
{
    public class UserForAuthenticationDTO
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password name is required")]
        public string Password { get; set; }
    }
}