using Microsoft.AspNetCore.Mvc;
using SwaggerFirstApp.Data;
using SwaggerFirstApp.Models;

namespace SwaggerFirstApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList();

            if(users.Count == 0)
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpGet("{id}")]

        public IActionResult GetUserById(int id) {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
