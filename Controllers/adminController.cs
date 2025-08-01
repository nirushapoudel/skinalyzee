using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skinalyze.API.Data;
using Skinalyze.API.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Skinalyze.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class adminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public adminController(AppDbContext context)
        {
            _context = context;
        }

        // ========== USERS ==========

        // GET: api/admin/users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.firstName,
                    u.lastName,
                    u.Email
                    // password excluded for security
                })
                .ToListAsync();

            return Ok(users);
        }

        // DELETE: api/admin/users/{id}
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ========== SKIN QUESTIONS ==========

        // GET: api/admin/questions
        [HttpGet("questions")]
        public async Task<IActionResult> GetAllQuestions()
        {
            var questions = await _context.SkinQuestionnaires.ToListAsync();
            return Ok(questions);
        }

        // POST: api/admin/questions
        [HttpPost("questions")]
        public async Task<IActionResult> AddQuestion([FromBody] SkinQuestionnaire question)
        {
            if (string.IsNullOrWhiteSpace(question.QuestionText))
                return BadRequest("QuestionText is required.");

            _context.SkinQuestionnaires.Add(question);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllQuestions), new { id = question.Id }, question);
        }

        // PUT: api/admin/questions/{id}
        [HttpPut("questions/{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, [FromBody] SkinQuestionnaire updatedQuestion)
        {
            var question = await _context.SkinQuestionnaires.FindAsync(id);
            if (question == null)
                return NotFound();

            question.QuestionText = updatedQuestion.QuestionText;
            await _context.SaveChangesAsync();
            return Ok(question);
        }

        // DELETE: api/admin/questions/{id}
        [HttpDelete("questions/{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.SkinQuestionnaires.FindAsync(id);
            if (question == null)
                return NotFound();

            _context.SkinQuestionnaires.Remove(question);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ========== PRODUCTS ==========

        // GET: api/admin/products
        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.products.ToListAsync();
            return Ok(products);
        }

        // POST: api/admin/products
        [HttpPost("products")]
        public async Task<IActionResult> AddProduct([FromBody] products product)
        {
            if (string.IsNullOrWhiteSpace(product.ProductName))
                return BadRequest("ProductName is required.");

            _context.products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllProducts), new { id = product.Id }, product);
        }

        // PUT: api/admin/products/{id}
        [HttpPut("products/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] products updatedProduct)
        {
            var product = await _context.products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.ProductName = updatedProduct.ProductName;
            product.Brand = updatedProduct.Brand;
            product.Category = updatedProduct.Category;
            product.Concern = updatedProduct.Concern;
            product.SkinType = updatedProduct.SkinType;
            product.Url = updatedProduct.Url;
            product.Price = updatedProduct.Price;

            await _context.SaveChangesAsync();
            return Ok(product);
        }

        // DELETE: api/admin/products/{id}
        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
