using SwaggerCodeFirstApp.Models;

namespace SwaggerFirstApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Product> Productos { get; set; } = new List<Product>();
    }
}
