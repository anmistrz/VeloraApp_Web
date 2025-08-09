using DealerApi.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DealerApi.Application.DTO;

namespace DealerApi.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserAuthServices _userAuthServices;

        public UserAuthController(IUserAuthServices userAuthServices)
        {
            _userAuthServices = userAuthServices ?? throw new ArgumentNullException(nameof(userAuthServices));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO userLoginDto)
        {
            if (userLoginDto == null)
            {
                return BadRequest(new { error = "Request body cannot be null or empty" });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                return BadRequest(new { errors });
            }

            try
            {
                var result = await _userAuthServices.LoginAsync(userLoginDto);
                if (result == null)
                {
                    return Unauthorized(new { error = "Invalid credentials" });
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO userRegisterDto)
        {
            if (userRegisterDto == null)
            {
                return BadRequest(new { error = "Request body cannot be null or empty" });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                return BadRequest(new { errors });
            }

            try
            {
                var result = await _userAuthServices.RegisterAsync(userRegisterDto);
                if (!result)
                {
                    return BadRequest(new { error = "Registration failed" });
                }
                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }


        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateDTO roleCreateDto)
        {
            if (roleCreateDto == null || string.IsNullOrEmpty(roleCreateDto.RoleName))
            {
                return BadRequest(new { error = "Role name cannot be null or empty" });
            }

            var roleName = roleCreateDto.RoleName.Trim();
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest(new { error = "Role name cannot be null or empty" });
            }

            try
            {
                var roleDto = new RoleCreateDTO { RoleName = roleName };
                var result = await _userAuthServices.CreateRoleAsync(roleDto);
                if (!result)
                {
                    return BadRequest(new { error = "Role creation failed" });
                }
                return Ok(new { message = "Role created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpPost("add-user-to-role")]
        public async Task<IActionResult> AddUserToRole([FromBody] RoleInsertDTO roleInsertDto)
        {
            if (roleInsertDto == null || string.IsNullOrEmpty(roleInsertDto.Email) || string.IsNullOrEmpty(roleInsertDto.RoleName))
            {
                return BadRequest(new { error = "Email and RoleName cannot be null or empty" });
            }

            try
            {
                var result = await _userAuthServices.AddUserToRoleAsync(roleInsertDto.Email, roleInsertDto.RoleName);
                if (!result)
                {
                    return BadRequest(new { error = "Failed to add user to role" });
                }
                return Ok(new { message = "User added to role successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpGet("get-roles-by-user")]
        public async Task<IActionResult> GetRolesByUser([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { error = "Email cannot be null or empty" });
            }

            try
            {
                var roles = await _userAuthServices.GetRolesByUserAsync(email);
                if (roles == null || !roles.Any())
                {
                    return NotFound(new { error = "No roles found for the user" });
                }
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
        
    }
}
