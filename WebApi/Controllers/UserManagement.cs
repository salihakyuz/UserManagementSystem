using Application.Contracts;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagement : ControllerBase
    {
        private readonly IUser userRepository;

        public UserManagement(IUser userRepository)
        {
            this.userRepository = userRepository;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userRepository.GetAllUsersAsync();
            return Ok(users);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut("{userId}/{newRole}")]
        public async Task<IActionResult> ChangeUserRole(int userId, RoleEnum newRole)
        {
            var result = await userRepository.ChangeUserRoleAsync(userId, newRole);
            if (result)
                return NoContent();
            return NotFound();
        }
        
        [HttpGet("address/{addressType}")]
        public async Task<IActionResult> GetUsersByAddress(AddressEnum addressType)
        {
            var users = await userRepository.GetUsersByAddressAsync(addressType);
            return Ok(users);
        }

        [HttpDelete("{userId}"),Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await userRepository.DeleteUserAsync(userId);
            if (result)
                return NoContent();
            return NotFound();
        }
    }
}
