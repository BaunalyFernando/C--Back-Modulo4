using SwaggerCodeFirstApp.Models;

namespace SwaggerFirstApp.Services
{
    public interface IProductService
    {
        Product GetProductById(int id);
    }
}
