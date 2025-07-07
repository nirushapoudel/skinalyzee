using Microsoft.AspNetCore.Mvc;
using Skinalyze.API.Data;
using Skinalyze.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Skinalyze.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthController(AppDbContext context)
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
                FirstName = registerDto.firstName,
                LastName = registerDto.lastName,
                Email = registerDto.email,
                password = registerDto.password,
                
            };

            registerDto.password = _passwordHasher.HashPassword(user, registerDto.password);


            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginUser)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginUser.Email);

            if (user == null)
                return Unauthorized("Invalid email");
            
            // Verify hashed password
            var result = _passwordHasher.VerifyHashedPassword(user, user.password, loginUser.password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid email or password");

            return Ok(new
            {
                Message = "Login successful",
                User = new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
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
    }
}
