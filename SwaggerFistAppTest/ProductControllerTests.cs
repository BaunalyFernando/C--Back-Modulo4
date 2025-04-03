using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwaggerCodeFirstApp.Models;
using SwaggerFirstApp.Data;
using SwaggerFirstApp.Services;
using SwaggerFirstApp.Services.DTOs;


public class ProductControllerTests
{
    private Mock<DbContext> _mockContext;
    private Mock<DbSet<Product>> _mockSet;
    private ProductsController _controller;
    private IProductService _productService;

    public ProductControllerTests()
    {
        _mockSet = new Mock<DbSet<Product>>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var context = new AppDbContext(options);

        _controller = new ProductsController(context, _productService);
    }

    [Fact]
    public void Create_ReturnsCreatedAtAction()
    {
        // Arrange
        var product = new ProductDto
                { Id = 1, 
                  Name = "Test Product", 
                  Price = 100 
                };

        // Act
        var result = _controller.Create(product) as CreatedAtActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(nameof(ProductsController.GetAll), result.ActionName);
        Assert.Equal(201, result.StatusCode);
        Assert.Equal(product, result.Value);
    }
}
