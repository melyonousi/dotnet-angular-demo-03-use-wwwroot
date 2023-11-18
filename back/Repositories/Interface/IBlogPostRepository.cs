using back.Models.Domain;

namespace back.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(IFormFile file, BlogPost blogPost);
        Task<BlogPost> UpdateAsync(BlogPost blogPost);
        Task<BlogPost> UpdateAsync(Guid id, string url);
        Task<BlogPost> DeleteAsync(Guid id);
        Task<BlogPost> GetByIdAsync(Guid id);
        Task<List<BlogPost>> GetAsync();
    }
}
