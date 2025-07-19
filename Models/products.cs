using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skinalyze.API.Models
{
    public class products
    {
        [Key]
        public int Id { get; set; }

        public string Category { get; set; }

        public string Url { get; set; }

        public string Brand { get; set; }

        [Column("product_name")]
        public string ProductName { get; set; }

        public string Price { get; set; }

        [Column("skin_type")]
        public string SkinType { get; set; }

        public string Concern { get; set; }
    }
}
