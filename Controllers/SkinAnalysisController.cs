using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class SkinAnalysisController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public SkinAnalysisController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost("analyze")]
    [Consumes("multipart/form-data")]

    public async Task<IActionResult> SkinAnalyze([FromForm] SkinAnalyzeRequest request)
    {
        if (request.image == null || request.image.Length == 0)
            return BadRequest("Image file is required.");

        try
        {
            using var content = new MultipartFormDataContent();
            using var stream = request.image.OpenReadStream();
            var imageContent = new StreamContent(stream);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue(request.image.ContentType);
            content.Add(imageContent, "image", request.image.FileName);

            var response = await _httpClient.PostAsync("http://localhost:5000/predict", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return Ok(result); // This is your model's response (e.g., JSON string)
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in skin analysis: {ex.Message}");
            return StatusCode(500, "Error analyzing skin image.");
        }
    }
}