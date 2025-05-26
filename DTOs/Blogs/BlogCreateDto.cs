namespace e_commerce.DTOs.Blogs
{
    public class BlogCreateDto
    {
        public Guid BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string Img { get; set; }
        public string ThumbnailImg { get; set; }
        public string Summary { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public string CommentsQuantity { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
        public string Description { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsPublished { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string CommenterName { get; set; }
        public string CommenterEmail { get; set; }
        public string Message { get; set; }
    }
}
