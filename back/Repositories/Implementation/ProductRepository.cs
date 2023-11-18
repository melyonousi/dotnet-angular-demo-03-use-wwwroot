using AutoMapper;
using back.Data;
using back.Helper;
using back.Models.Domain;
using back.Models.DTO;
using back.Repositories.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace back.Repositories.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly BaseUrlService _baseUrlService;
        private readonly ILogger<ProductRepository> _logger;
        private readonly IUploadRepository _uploadRepository;

        public ProductRepository(AppDbContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment, BaseUrlService baseUrlService, ILogger<ProductRepository> logger, IUploadRepository uploadRepository)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _baseUrlService = baseUrlService;
            _logger = logger;
            _uploadRepository = uploadRepository;
        }
        public async Task<Product> CreateAsync(IFormFile file, Product _product)
        {
            Product product = new()
            {
                Title = _product.Title,
                Description = _product.Description,
                Thumbnail = _product.Thumbnail,
                PublishedDate = _product.PublishedDate,
                Visibility = _product.Visibility,
            };

            await _dbContext.Products.AddAsync(product);

            product.Thumbnail = await _uploadRepository.UploadAsync(file, product.Id.ToString(), FolderName.ProductsFolder);

            await _dbContext.SaveChangesAsync();

            return product;
        }

        public async Task<Product> DeleteAsync(Guid id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return null!;
            }

            await _uploadRepository.RemoveAsync(product.Id.ToString(), FolderName.ProductsFolder, product.Thumbnail!);


            _dbContext.Products.Remove(product);

            await _dbContext.SaveChangesAsync();


            return product;
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return null!;
            }

            var newProduct = new Product
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Thumbnail = Path.Combine(_baseUrlService.BaseUrl, product.Thumbnail!),
                PublishedDate = product.PublishedDate,
                Visibility = product.Visibility,
            };

            return newProduct;
        }

        public async Task<List<ProductEntity>> GetAsync()
        {
            var _products = await _dbContext.Products.ToListAsync();

            if (_products == null) return null!;

            var _productsWithThumnailPath = _products.Select(product => product.Thumbnail = Path.Combine(_baseUrlService.BaseUrl, product.Thumbnail!));

            List<ProductEntity> productEntities = _products.Select(product =>
            {
                product.Thumbnail = Path.Combine(_baseUrlService.BaseUrl, product.Thumbnail!);
                return _mapper.Map<Product, ProductEntity>(product);
            }).ToList();

            return productEntities;
        }

        public async Task<Product> UpdateAsync(IFormFile file, Product _product)
        {
            var product = await _dbContext.Products.FindAsync(_product.Id);

            if (product == null)
            {
                return null!;
            }

            product.Title = _product.Title;
            product.Description = _product.Description;
            product.Thumbnail = await _uploadRepository.UploadAsync(file, product.Id.ToString(), FolderName.ProductsFolder);
            product.PublishedDate = _product.PublishedDate;
            product.Visibility = _product.Visibility;

            await _dbContext.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateAsync(Guid id, string url)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null || string.IsNullOrEmpty(url))
            {
                return null!;
            }


            product.Thumbnail = url;

            await _dbContext.SaveChangesAsync();

            return product;
        }
    }
}
