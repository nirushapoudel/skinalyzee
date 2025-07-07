using System.ComponentModel.DataAnnotations.Schema;

namespace Skinalyze.API.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string SkinType { get; set; }
        public string Ingredients { get; set; }
        public string ProductType { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public string URL { get; set; }
        public float Rating { get; set; }
    }
}
