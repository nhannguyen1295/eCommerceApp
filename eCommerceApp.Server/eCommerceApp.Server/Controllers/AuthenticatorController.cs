using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using eCommerceApp.Contract;
using eCommerceApp.Entities.DTO;
using eCommerceApp.Entities.Models;
using eCommerceApp.Entities.Models.Identity;
using eCommerceApp.Server.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApp.Server.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticatorController : ControllerBase
    {
        private readonly ILoggerManager _loggerManager;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationManager _authManager;

        public AuthenticatorController(ILoggerManager loggerManager,
                                       IMapper mapper,
                                       UserManager<User> userManager,
                                       IAuthenticationManager authManager)
        {
            _loggerManager = loggerManager;
            _mapper = mapper;
            _userManager = userManager;
            _authManager = authManager;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDTO userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            await _userManager.AddToRolesAsync(user, userForRegistration.Roles);

            return StatusCode(201);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDTO user)
        {
            if (!await _authManager.ValidateUser(user))
            {
                _loggerManager.LogWarn($"{nameof(Authenticate)}: Authentication failed, wrong user name or password");
                return Unauthorized();
            }

            return Ok(new { Token = await _authManager.CreateToken() });
        }

        // For normal user
        [HttpPut, Authorize]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> UpdateAccount([FromBody] UserForUpdateDTO userForRolesManagerDTO)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await _userManager.FindByIdAsync(id.ToString());

            user.FirstName = userForRolesManagerDTO.FirstName == null ? user.FirstName : userForRolesManagerDTO.FirstName;
            user.LastName = userForRolesManagerDTO.LastName == null ? user.LastName : userForRolesManagerDTO.LastName;
            user.Email = userForRolesManagerDTO.Email == null ? user.Email : userForRolesManagerDTO.Email;
            user.PhoneNumber = userForRolesManagerDTO.PhoneNumber == null ? user.PhoneNumber : userForRolesManagerDTO.PhoneNumber;
            if (userForRolesManagerDTO.Password != null)
            {
                var newPassword = _userManager.PasswordHasher.HashPassword(user, userForRolesManagerDTO.Password);
                user.PasswordHash = newPassword;
            }

            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        // For administrator
        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> UpdateRoles(Guid Id, [FromBody] UserForRolesManagerDTO userForRolesManagerDTO)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (userForRolesManagerDTO.Roles.Count != 0)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                foreach (var role in userForRolesManagerDTO.Roles)
                {
                    var isInRole = await _userManager.IsInRoleAsync(user, role);
                    if (!isInRole)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                }
            }

            await _userManager.UpdateAsync(user);

            return NoContent();
        }
    }
}