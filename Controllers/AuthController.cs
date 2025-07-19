using Microsoft.AspNetCore.Mvc;
using Skinalyze.API.Data;
using Skinalyze.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Skinalyze.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class authController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public authController(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.email))
                return BadRequest("Email already exists");
            if(string.IsNullOrWhiteSpace(registerDto.password))
            {
                return BadRequest("Password is required");
            }    
            if (registerDto.password != registerDto.confirmPassword)
            {
                return BadRequest("Passwords do not match");
            }
            var user = new User
            {
                firstName = registerDto.firstName,
                lastName = registerDto.lastName,
                Email = registerDto.email,
                password = registerDto.password,
                
            };

            user.password = _passwordHasher.HashPassword(user, registerDto.password);


            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.email);

            if (user == null)
                return Unauthorized("Invalid email");
            
            // Verify hashed password
            var result = _passwordHasher.VerifyHashedPassword(user, user.password, loginDto.password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid email or password");

            return Ok(new
            {
                Message = "Login successful",
                User = new
                {
                    user.Id,
                    user.firstName,
                    user.lastName,
                    user.Email,
                    user.password,
                    //user.Confirmpassword
                }
            });
        }

        public class RegisterDto
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string email { get; set; }
            public string password { get; set; }
            public string confirmPassword { get; set; }

        }

        public class LoginDto
        {
            public string email { get; set; }
            public string password { get; set; }
        }
    }
}
