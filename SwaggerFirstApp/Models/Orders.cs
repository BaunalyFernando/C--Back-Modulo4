using SwaggerCodeFirstApp.Models;

namespace SwaggerFirstApp.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public User User { get; set; }

        public Product Product { get; set; }
    }
}
