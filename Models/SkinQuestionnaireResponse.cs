using System.Collections.Generic;

namespace Skinalyze.API.Models
{
    public class SkinQuestionnaireResponse
    {
        public string SkinType { get; set; }
        public List<string> Products { get; set; }
    }
}
