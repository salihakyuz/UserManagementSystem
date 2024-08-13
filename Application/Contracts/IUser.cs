
using Application.DTOs;
using Domain.Entities;
using Domain.Enums;

namespace Application.Contracts
{
    public interface IUser
    {
        Task<RegistrationResponse> RegisterUserAsync(RegisterUserDTO registerUserDTO);
        Task<LoginResponse> LoginUserAsync(LoginDTO loginDTO);
        Task<IEnumerable<User>> GetAllUsersAsync();//userdto olabilir
        Task<bool> ChangeUserRoleAsync(int userId, RoleEnum newRole);
        Task<IEnumerable<UserAddressDTO>> GetUsersByAddressAsync(AddressEnum addressType);//useraddresdto ile değişti normalde useraddress vardı
        Task<bool> DeleteUserAsync(int userId);
    }
}
