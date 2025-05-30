﻿using e_commerce.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static e_commerce.Enums.ProductEnums;

namespace e_commerce.DTOs.Products
{
    public class ProductReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; } 
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal RegularPrice { get; set; }
        public int Quantity { get; set; }
        public int ReviewQuantity { get; set; }
        public List<string> Categories { get; set; }
        public List<string> SubCategories { get; set; }
        public List<string> Tags { get; set; }
        public DateTime Date { get; set; } 
        public ProductStatus Status { get; set; } 
        public decimal Rating { get; set; }
        public List<string> Images { get; set; }
        public string Description { get; set; } 
        public string Badge { get; set; } 
        public string Brand { get; set; }
        public string Color { get; set; }
        public string Model { get; set; } 
        public string Sku { get; set; } 
        public List<string> ShortFeatures { get; set; }
        public string Specifications { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
    }
}
