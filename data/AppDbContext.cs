using e_commerce.Models.Account;
using e_commerce.Models.Blogs;
using e_commerce.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.data
{
    public class AppDbContext: DbContext
    {
        public  AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<UserRegisterModel> Users { get; set; }
        public DbSet<BlogModel> Blogs { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRegisterModel>()
                .HasAlternateKey(u => u.Email);

            modelBuilder.Entity<UserRegisterModel>()
                .HasAlternateKey(u => u.Username);

            modelBuilder.Entity<UserRegisterModel>()
                .HasAlternateKey(u => new { u.Email, u.Username });
        }
    }
}

