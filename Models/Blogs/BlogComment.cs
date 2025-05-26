using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models.Blogs
{
    public class BlogComment
    {
        [Key]
        public Guid CommentId { get; set; }
        public Guid BlogId { get; set; }
        public string CommenterName { get; set; }
        public string CommenterEmail { get; set; }
        public string Message { get; set; }
        public DateTime CommentedAt { get; set; }
    }

}
