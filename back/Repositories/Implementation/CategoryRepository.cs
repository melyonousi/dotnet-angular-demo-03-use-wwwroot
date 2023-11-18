using back.Data;
using back.Models.Domain;
using back.Repositories.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace back.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _dbContext;

        public CategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<Category> DeleteAsync(Guid id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return category!;
            }

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null)
            {
                return category!;
            }

            return category!;
        }

        public async Task<List<Category>> GetAsync()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            return categories!;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var ctg = await _dbContext.Categories.FindAsync(category.Id);

            if (ctg == null)
            {
                return ctg!;
            }

            ctg.Name = category.Name;
            ctg.UrlHandle = category.UrlHandle;

            await _dbContext.SaveChangesAsync();

            return ctg;
        }
    }
}
