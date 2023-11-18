using back.Data;
using back.Helper;
using back.Models.Domain;
using back.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace back.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUploadRepository _uploadRepository;

        public BlogPostRepository(AppDbContext appDbContext, IUploadRepository uploadRepository)
        {
            _appDbContext = appDbContext;
            _uploadRepository = uploadRepository;
        }

        public async Task<BlogPost> CreateAsync(IFormFile file, BlogPost _blogPost)
        {
            BlogPost blogPost = new BlogPost
            {
                Author = _blogPost.Author,
                FeaturedImageUrl = _blogPost.FeaturedImageUrl,
                Title = _blogPost.Title,
                Content = _blogPost.Content,
                IsVisible = _blogPost.IsVisible,
                PublishedDate = _blogPost.PublishedDate,
                ShortDescription = _blogPost.ShortDescription,
                UrlHandle = _blogPost.UrlHandle,
            };

            await _appDbContext.BlogPosts.AddAsync(blogPost);

            blogPost.FeaturedImageUrl = await _uploadRepository.UploadAsync(file, blogPost.Id.ToString(), FolderName.BlogPostsFolder);

            await _appDbContext.SaveChangesAsync();

            return blogPost;
        }

        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            var blgPst = await _appDbContext.BlogPosts.FindAsync(blogPost.Id);

            if (blgPst == null)
            {
                return blgPst!;
            }

            blgPst.Title = blogPost.Title;
            blgPst.ShortDescription = blogPost.ShortDescription;
            blgPst.Content = blogPost.Content;
            blgPst.FeaturedImageUrl = blogPost.FeaturedImageUrl;
            blgPst.UrlHandle = blogPost.UrlHandle;
            blgPst.PublishedDate = blogPost.PublishedDate;
            blgPst.Author = blogPost.Author;
            blgPst.IsVisible = blogPost.IsVisible;

            await _appDbContext.SaveChangesAsync();

            return blgPst;
        }

        public async Task<BlogPost> DeleteAsync(Guid id)
        {
            var blogPost = await _appDbContext.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return blogPost!;
            }

            await _uploadRepository.RemoveAsync(blogPost.Id.ToString(), FolderName.BlogPostsFolder, blogPost.FeaturedImageUrl);

            _appDbContext.BlogPosts.Remove(blogPost);
            await _appDbContext.SaveChangesAsync();

            return blogPost;
        }

        public async Task<List<BlogPost>> GetAsync()
        {
            var blogPosts = await _appDbContext.BlogPosts.ToListAsync();
            return blogPosts!;
        }

        public async Task<BlogPost> GetByIdAsync(Guid id)
        {
            var blogPosts = await _appDbContext.BlogPosts.FindAsync(id);
            if (blogPosts == null)
            {
                return blogPosts!;
            }
            return blogPosts!;
        }

        public async Task<BlogPost> UpdateAsync(Guid id, string url)
        {
            var product = await _appDbContext.BlogPosts.FindAsync(id);

            if (product == null || string.IsNullOrEmpty(url))
            {
                return null!;
            }

            product.FeaturedImageUrl = url;

            await _appDbContext.SaveChangesAsync();

            return product;
        }
    }
}
