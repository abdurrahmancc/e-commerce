using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.Products
{
    public class ProductModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Product Id is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product name is must be at least 2 character long")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Product Name is required")]
        public int Price { get; set; }
        public int RegularPrice { get; set; }
        public int Quantity { get; set; }
        public int ReviewQuantity { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Review { get; set; }
        public string Img { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
