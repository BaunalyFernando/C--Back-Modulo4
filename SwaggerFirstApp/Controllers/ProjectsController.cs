using Microsoft.AspNetCore.Mvc;
using Polly;
using SwaggerFirstApp.Data;



    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var projects = _context.Projects.ToList();
            return Ok(projects);
        }
    }

