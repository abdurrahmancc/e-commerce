using e_commerce.Models.Account;
using e_commerce.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.data
{
    public class AppDbContext: DbContext
    {
        public  AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<UserModel> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasAlternateKey(u => u.Email);

            modelBuilder.Entity<UserModel>()
                .HasAlternateKey(u => u.Username);

            modelBuilder.Entity<UserModel>()
                .HasAlternateKey(u => new { u.Email, u.Username });
        }
    }
}
