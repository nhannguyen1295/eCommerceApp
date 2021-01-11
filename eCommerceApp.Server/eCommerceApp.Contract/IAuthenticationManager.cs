using System.Threading.Tasks;
using eCommerceApp.Entities.DTO;

namespace eCommerceApp.Contract
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(UserForAuthenticationDTO userForAuthenticationDTO);
        Task<string> CreateToken();
    }
}