using System.Collections.Generic;

namespace eCommerceApp.Entities.DTO
{
    public class UserForRolesManagerDTO
    {
        public ICollection<string> Roles { get; set; }
    }
}