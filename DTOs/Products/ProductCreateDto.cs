using e_commerce.Enums;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.DTOs.Products
{
    public class ProductCreateDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Product Id is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product name is must be at least 2 character long")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Product FullName is required")]
        public string FullName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public decimal RegularPrice { get; set; }
        public int Quantity { get; set; }
        public int ReviewQuantity { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
        public List<string> SubCategories { get; set; }
        public DateTime Date { get; set; } = DateTime.MinValue;
        public string Status { get; set; } = string.Empty;
        public double Rating { get; set; } = 0;
        [Required]
        public List<IFormFile> Images { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Badge { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public List<string> ShortFeatures { get; set; } = new List<string>();
        [NotMapped]
        public Dictionary<string, List<string>> Specifications { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public DateTime UpdatedAt { get; set; } = DateTime.MinValue;
    }
}
