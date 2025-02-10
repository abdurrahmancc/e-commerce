using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.DTOs.Products
{
    public class ProductReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; } 
        public decimal Price { get; set; }
        public decimal RegularPrice { get; set; }
        public int Quantity { get; set; }
        public int ReviewQuantity { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; } 
        public DateTime Date { get; set; } 
        public string Status { get; set; } 
        public int Review { get; set; } = 0;
        public List<string> Img { get; set; }
        public string Description { get; set; } 
        public string Badge { get; set; } 
        public string Model { get; set; } 
        public string Sku { get; set; } 
        public List<string> ShortFeatures { get; set; }
        public string Specifications { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
    }
}
