using System;
using DealerApi.Application.DTO;

namespace DealerApi.Application.Interface;

public interface IUserAuthBL
{
    //registration asp identity
    Task<bool> RegisterAsync(RegistrationDTO registrationDTO);
    //login asp identity
    Task<UserWithTokenDTO> LoginAsync(LoginDTO loginDTO);
    //create role
    Task<bool> CreateRoleAsync(RoleCreateDTO roleCreateDTO);
    //add user to role
    Task<bool> AddUserToRoleAsync(string email, string roleName);

    //get roles by user
    Task<List<string>> GetRolesByUserAsync(string email);
}
