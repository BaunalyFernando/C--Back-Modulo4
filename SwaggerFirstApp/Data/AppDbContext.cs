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

        // Define los DbSet para tus entidades
        public DbSet<Product> Products { get; set; }
        public DbSet<Projects> Projects { get; set; }
    }
}
