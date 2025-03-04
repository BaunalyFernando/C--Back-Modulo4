using Microsoft.AspNetCore.Mvc;
using SwaggerFirstApp.Data;
using SwaggerFirstApp.Services.DTOs;

namespace SwaggerFirstApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _context.Categories.ToList();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public IActionResult Create(CategoryDto categoryDto)
        {
            var category = new Models.Category
            {
                Name = categoryDto.Name
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
