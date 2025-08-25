using System;
using Microsoft.AspNetCore.Identity;

namespace DealerApi.DAL.Interfaces;

public interface IUserAuth
{
        //registration asp identity
        Task<bool> RegisterAsync(string firstName, string lastName, string username, string email, string password);
        //login asp identity
        Task<IdentityUser> LoginAsync(string email, string password);
        //create role
        Task<bool> CreateRoleAsync(string roleName);
        //add user to role
        Task<bool> AddUserToRoleAsync(string email, string roleName);
        Task<List<string>> GetRolesByUserAsync(string email);
        Task<bool> DeleteRoleInUserAsync(string email, string roleName);
}
