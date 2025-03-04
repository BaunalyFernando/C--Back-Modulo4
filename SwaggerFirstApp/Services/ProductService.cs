using SwaggerCodeFirstApp.Models;
using SwaggerFirstApp.Data;

namespace SwaggerFirstApp.Services
{
    internal class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public Product GetProductById(int id)
        {
            var product =  _context.Products.Find(id);

            if(product == null) {
                return null;
            }

            return product;
        }
    }
}
