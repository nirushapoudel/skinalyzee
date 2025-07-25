using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skinalyze.API.Data;
using Skinalyze.API.Models;

namespace Skinalyze.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class productsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public productsController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/products?page=1&pageSize=15
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 15)
        {
            var total = await _context.products.CountAsync();
            var data = await _context.products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                total,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling((double)total / pageSize),
                data
            });
        }

        // ✅ GET: api/products/recommend?category=face-moisturisers&concern=hydration
        [HttpGet("recommend")]
        public async Task<IActionResult> Recommend(string category, string concern)
        {
            if (string.IsNullOrEmpty(category) || string.IsNullOrEmpty(concern))
                return BadRequest("Category and concern must be provided.");

            var data = await _context.products
                .Where(p =>
                    p.Category.ToLower().Contains(category.ToLower()) &&
                    p.Concern.ToLower().Contains(concern.ToLower()))
                .Take(10)
                .ToListAsync();

            if (!data.Any())
                return NotFound("No recommendations found.");

            return Ok(data);
        }

        // ✅ GET: /api/products/search?query=ponds
        [HttpGet("search")]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Search query is required.");

            var data = await _context.products
                .Where(p =>
                    p.Brand.ToLower().Contains(query.ToLower()) ||
                    p.ProductName.ToLower().Contains(query.ToLower()) |
                    p.Category.ToLower().Contains(query.ToLower()))
                .ToListAsync();

            return Ok(data);
        }


        // ✅ GET: api/products/skin?type=oily
        [HttpGet("skin")]
        public async Task<IActionResult> RecommendBySkinType(string type)
        {
            if (string.IsNullOrEmpty(type))
                return BadRequest(new { message = "Skin type must be provided." });

            var data = await _context.products
                .Where(p => p.SkinType.ToLower().Contains(type.ToLower()))
                .ToListAsync();
            // Always return valid JSON
            if (!data.Any())
                return Ok(new List<products>()); // Return empty list instead of plain string

            return Ok(data);
        }
    }
}
