using Microsoft.AspNetCore.Mvc;
using SwaggerFirstApp.Data;
using SwaggerCodeFirstApp.Models;
using SwaggerFirstApp.Services;
using SwaggerFirstApp.Services.DTOs;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    public readonly IProductService _productsService;

    public ProductsController(AppDbContext context, IProductService productService)
    {
        _context = context;
        _productsService = productService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var products = _context.Products.ToList();
        return Ok(products);
    }

    [HttpPost]
    public IActionResult Create(ProductDto productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Price = productDto.Price,
            CategoryId = productDto.CategoryId
        };
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
        var product = _productsService.GetProductById(id);

        if (product == null)
        {
            return NotFound();
        }
            
        return Ok(product);
    }
}
