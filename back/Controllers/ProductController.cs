using back.Data;
using back.Models.Domain;
using back.Models.DTO;
using back.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "user")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _contextRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly BaseUrlService _baseUrlService;
        private readonly IUploadRepository _uploadRepository;
        private readonly ILogger<Product> _logger;

        public ProductsController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment, BaseUrlService baseUrlService, IUploadRepository uploadRepository, ILogger<Product> logger)
        {
            _contextRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
            _baseUrlService = baseUrlService;
            _uploadRepository = uploadRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var _products = await _contextRepository.GetAsync();
            if (_products == null && _products!.Count <= 0)
            {
                return BadRequest();
            }

            return Ok(_products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProducts(Guid id)
        {
            Product product = await _contextRepository.GetByIdAsync(id);
            return product == null ? NotFound(id) : Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducts(Guid id)
        {
            Product product = await _contextRepository.DeleteAsync(id);
            return product == null ? NotFound(product) : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducts([FromForm] IFormFile file, [FromForm] Product _product)
        {
            Product product = await _contextRepository.CreateAsync(file, _product);

            return product == null ? BadRequest(_product) : Ok(product);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProducts(IFormFile file, Product _product)
        {
            Product product = await _contextRepository.UpdateAsync(file, _product);

            return product == null ? NotFound(_product) : Ok(product);
        }
    }
}
