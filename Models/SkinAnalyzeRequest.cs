using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class SkinAnalyzeRequest
{
    [Required]
    public IFormFile? image { get; set; }
}
