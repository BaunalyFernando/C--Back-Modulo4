using SwaggerFirstApp.Data;

namespace SwaggerFirsAppService.ProductsService
{

    internal class ProductsService : IProductsService
    {
        private readonly AppDbContext _context;
        public int GetProductById(int id)
        {
            _context.Products.Find(id);

            return id;
        }
    }
}
