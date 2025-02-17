using Microsoft.AspNetCore.Mvc;
using SwaggerFirstApp.Data;
using SwaggerCodeFirstApp.Models;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var products = _context.Products.ToList();
        return Ok(products);
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAll), new { id = product.Id }, product);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }
}
