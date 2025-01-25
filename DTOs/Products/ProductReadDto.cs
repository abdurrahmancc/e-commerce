using System.ComponentModel.DataAnnotations;

namespace e_commerce.DTOs.Products
{
    public class ProductReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}
