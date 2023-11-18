using back.Models.Domain;
using back.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace back.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(IFormFile file, Product _product);
        Task<Product> UpdateAsync(IFormFile file, Product product);
        Task<Product> UpdateAsync(Guid id, string url);
        Task<Product> DeleteAsync(Guid id);
        Task<Product> GetByIdAsync(Guid id);
        Task<List<ProductEntity>> GetAsync();
    }
}
