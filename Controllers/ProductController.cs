using Microsoft.AspNetCore.Mvc;
using Skinalyze.API.Data;
using Skinalyze.API.Models;
using Microsoft.EntityFrameworkCore;
using SkinalyzeApi.Models;

namespace Skinalyze.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: /api/products/skin/oily
        [HttpGet("skin/{type}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetBySkinType(string type)
        {
            return await _context.Products
                .Where(p => p.SkinType.ToLower() == type.ToLower())
                .ToListAsync();
        }

        // ✅ POST: /api/products/recommend
        [HttpPost("recommend")]
        public async Task<ActionResult<IEnumerable<Product>>> Recommend([FromBody] RecommendationRequest request)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(request.SkinType))
            {
                query = query.Where(p => p.SkinType.ToLower() == request.SkinType.ToLower());
            }

            //if (!string.IsNullOrEmpty(request.Condition))
            //{
            //    query = query.Where(p => p.Ingredients.ToLower().Contains(request.Condition.ToLower()));
            //}

            var results = await query
                .OrderByDescending(p => p.Rating)
                .Take(5)
                .ToListAsync();

            if (!results.Any())
            {
                return NotFound("No matching products found.");
            }

            return Ok(results);
        }
    }
}
