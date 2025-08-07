using System;
using System.Security.Claims;
using DealerApi.Application.DTO;
using DealerApi.Application.Helpers;
using DealerApi.Application.Interface;
using DealerApi.DAL.Interfaces;
using Microsoft.Extensions.Options;

namespace DealerApi.Application.Services;

public class UserAuthServices : IUserAuthServices
{
    private readonly IUserAuth _userAuthDAL;
    private readonly AppSettings _appSettings;

    public UserAuthServices(IUserAuth userAuthDAL, IOptions<AppSettings> appSettings)
    {
        _userAuthDAL = userAuthDAL ?? throw new ArgumentNullException(nameof(userAuthDAL));
        _appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));
    }


    public async Task<bool> AddUserToRoleAsync(string email, string roleName)
    {
       try 
       {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Role name cannot be null or empty", nameof(roleName));
            }

            return await _userAuthDAL.AddUserToRoleAsync(email, roleName);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error adding user to role: {ex.Message}", ex);
        }
    }

    public async Task<bool> CreateRoleAsync(RoleCreateDTO roleCreateDTO)
    {
        try
        {
            if (roleCreateDTO == null)
            {
                throw new ArgumentNullException(nameof(roleCreateDTO), "RoleCreateDTO cannot be null");
            }
            if (string.IsNullOrWhiteSpace(roleCreateDTO.RoleName))
            {
                throw new ArgumentException("Role name cannot be null or empty", nameof(roleCreateDTO.RoleName));
            }

            return await _userAuthDAL.CreateRoleAsync(roleCreateDTO.RoleName);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error creating role: {ex.Message}", ex);
        }
    }

    public async Task<List<string>> GetRolesByUserAsync(string email)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            return await _userAuthDAL.GetRolesByUserAsync(email);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error retrieving roles for user: {ex.Message}", ex);
        }
    }

    public async Task<UserWithTokenDTO> LoginAsync(LoginDTO loginDTO)
    {
        try
        {
            if (loginDTO == null)
            {
                throw new ArgumentNullException(nameof(loginDTO), "LoginDTO cannot be null");
            }

            var user = await _userAuthDAL.LoginAsync(loginDTO.Email, loginDTO.Password);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid login credentials.");
            }

            //List Claim
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            var roles = await _userAuthDAL.GetRolesByUserAsync(user.Email);
            if (roles != null && roles.Count > 0)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                    Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var userWithToken = new UserWithTokenDTO
            {
                Email = user.Email,
                Token = tokenHandler.WriteToken(token)
            };
            Console.WriteLine($"User {user.Email} logged in successfully. Token: {userWithToken.Token}");
            return userWithToken;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error logging in: {ex.Message}", ex);
        }
    }

    public Task<bool> RegisterAsync(RegistrationDTO registrationDTO)
    {
        try
        {
            if (registrationDTO == null)
            {
                throw new ArgumentNullException(nameof(registrationDTO), "RegistrationDTO cannot be null");
            }
            if (string.IsNullOrWhiteSpace(registrationDTO.Email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(registrationDTO.Email));
            }
            if (string.IsNullOrWhiteSpace(registrationDTO.Password))
            {
                throw new ArgumentException("Password cannot be null or empty", nameof(registrationDTO.Password));
            }

            return _userAuthDAL.RegisterAsync(registrationDTO.UserName, registrationDTO.Email, registrationDTO.Password);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error registering user: {ex.Message}", ex);
        }
    }
}
