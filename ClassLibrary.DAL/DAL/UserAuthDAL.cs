using System;
using DealerApi.DAL.Context;
using DealerApi.DAL.Interfaces;
using DealerApi.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace DealerApi.DAL.DAL;

public class UserAuthDAL : IUserAuth
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly DealerRndDBContext _context;

    public UserAuthDAL(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, DealerRndDBContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }


    public async Task<bool> AddUserToRoleAsync(string email, string roleName)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                throw new ArgumentException("Role does not exist");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error adding user to role: {ex.Message}", ex);
        }
    }

    public async Task<bool> CreateRoleAsync(string roleName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Role name cannot be null or empty");
            }
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                throw new ArgumentException("Role already exists");
            }
            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error creating role: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteRoleInUserAsync(string email, string roleName)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                throw new ArgumentException("Role does not exist");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error deleting role from user: {ex.Message}", ex);
        }
    }

    public async Task<List<string>> GetRolesByUserAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error retrieving roles for user: {ex.Message}", ex);
        }
    }

    public async Task<IdentityUser> LoginAsync(string email, string password)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result)
            {
                throw new ArgumentException("Invalid password");
            }

            return user;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error logging in user: {ex.Message}", ex);
        }
    }

    public async Task<bool> RegisterAsync(string firstName, string lastName, string username, string email, string password)
    {
        try
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var user = new IdentityUser { Email = email, UserName = username };
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("User registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                var createCustomerVerified = new CustomerVerified
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                };

                _context.CustomerVerifieds.Add(createCustomerVerified);
                await _context.SaveChangesAsync();

                if (!await _roleManager.RoleExistsAsync("customer"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("customer"));
                }

                await _userManager.AddToRoleAsync(user, "customer");
                await _context.SaveChangesAsync();

                await _context.Database.CommitTransactionAsync();

                return result.Succeeded;
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error registering user: {ex.Message}", ex);
        }
    }
}
