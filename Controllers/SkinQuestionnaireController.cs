using Microsoft.AspNetCore.Mvc;
using Skinalyze.API.Models;
using System.Collections.Generic;
using System.Linq;

namespace Skinalyze.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkinQuestionnaireController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] SkinQuestionnaireRequest request)
        {
            if (request?.Answers == null || request.Answers.Count == 0)
                return BadRequest("Answers are required.");

            // Count occurrences of each skin type
            var counts = new Dictionary<string, int>
            {
                { "dry", 0 },
                { "normal", 0 },
                { "oily", 0 }
            };

            foreach (var answer in request.Answers)
            {
                if (counts.ContainsKey(answer))
                    counts[answer]++;
            }

            var skinType = counts.OrderByDescending(c => c.Value).First().Key;

            var products = skinType switch
            {
                "dry" => new List<string>
{
    "CeraVe Hydrating Cleanser",
    "Klairs Supple Preparation Facial Toner",
    "The Ordinary Hyaluronic Acid 2% + B5 Serum",
    "Neutrogena Hydro Boost Gel-Cream Moisturizer",
    "La Roche-Posay Anthelios Melt-in Milk Sunscreen SPF 100"
},

                "normal" => new List<string>
{
    "Cetaphil Gentle Skin Cleanser",
    "Paula's Choice Skin Balancing Pore-Reducing Toner",
    "The Ordinary Niacinamide 10% + Zinc 1% Serum",
    "Clinique Dramatically Different Moisturizing Lotion+",
    "Neutrogena Ultra Sheer Dry-Touch Sunscreen SPF 55"
},

                "oily" => new List<string>
{
    "La Roche-Posay Effaclar Purifying Foaming Gel Cleanser",
    "Thayers Witch Hazel Toner",
    "The Ordinary Niacinamide 10% + Zinc 1% Serum",
    "Belif The True Cream Aqua Bomb",
    "EltaMD UV Clear Broad-Spectrum SPF 46"
},

                _ => new List<string>
{
    "Simple Kind to Skin Moisturizing Facial Wash",
    "Fresh Rose Deep Hydration Facial Toner",
    "Estée Lauder Advanced Night Repair Serum",
    "Olay Regenerist Micro-Sculpting Cream",
    "Aveeno Positively Radiant Daily Moisturizer with SPF 30"
}

            };

            return Ok(new SkinQuestionnaireResponse
            {
                SkinType = skinType,
                Products = products
            });
        }
    }
}
