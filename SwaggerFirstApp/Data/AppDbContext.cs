using Microsoft.EntityFrameworkCore;
using SwaggerCodeFirstApp.Models;
using SwaggerFirstApp.Models;

namespace SwaggerFirstApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Projects> Projects { get; set; }

        public DbSet<Category> Categories { get; set; }


    }
}
