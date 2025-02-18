using Microsoft.AspNetCore.Mvc;
using SwaggerFirstApp.Data;
using SwaggerFirstApp.Models;

namespace SwaggerFirstApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController: ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var orders = _context.Orders.ToList();
            return Ok();
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public IActionResult Create(Orders order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }
    }
}
