using Core.Entities.Identity;
using Core.Interfaces;
using FileBankAPI.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FileBankAPI.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto model)
        {
            // Check if roles exist, create them if necessary
            foreach (var roleName in model.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var user = new AppUser
            {
                UserName = model.Username,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Assign roles to the user
                await _userManager.AddToRolesAsync(user, model.Roles);

                // Generate and return a JWT token for the registered user (similar to login)

                var token = _tokenService.CreateToken(user);

                return Ok(new { Token = token });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] UserLoginDto model)
        {
            // Authenticate user
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Invalid username or password");
            }

            // Generate JWT token
            return new UserDto()
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };
        }

        /*private string GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            foreach (var role in _userManager.GetRolesAsync(user).Result)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }*/
    }
}
