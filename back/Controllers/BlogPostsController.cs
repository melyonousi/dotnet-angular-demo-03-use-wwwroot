using back.Models.Domain;
using back.Models.DTO;
using back.Repositories.Implementation;
using back.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly IUploadRepository _uploadRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, IWebHostEnvironment env, IUploadRepository uploadRepository)
        {
            _blogPostRepository = blogPostRepository;
            _hostingEnv = env;
            _uploadRepository = uploadRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPosts([FromForm] IFormFile file, [FromForm] CreateBlogPostRequestDTO createBlogPostRequest)
        {
            var blogPost = new BlogPost
            {
                Title = createBlogPostRequest.Title,
                ShortDescription = createBlogPostRequest.ShortDescription,
                Content = createBlogPostRequest.Content,
                FeaturedImageUrl = createBlogPostRequest.FeaturedImageUrl,
                UrlHandle = createBlogPostRequest.UrlHandle,
                PublishedDate = createBlogPostRequest.PublishedDate,
                Author = createBlogPostRequest.Author,
                IsVisible = createBlogPostRequest.IsVisible
            };

            var blgPost = await _blogPostRepository.CreateAsync(file, blogPost);

            if (blgPost == null)
            {
                return NotFound($"cannot save this blog post");
            }

            var response = new BlogPostDTO
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible
            };

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBlogPosts(UpdateBlogPostRequestDTO updateBlogPostRequestDTO)
        {
            var blogPost = new BlogPost
            {
                Id = updateBlogPostRequestDTO.Id,
                Title = updateBlogPostRequestDTO.Title,
                ShortDescription = updateBlogPostRequestDTO.ShortDescription,
                Content = updateBlogPostRequestDTO.Content,
                FeaturedImageUrl = updateBlogPostRequestDTO.FeaturedImageUrl,
                UrlHandle = updateBlogPostRequestDTO.UrlHandle,
                PublishedDate = updateBlogPostRequestDTO.PublishedDate,
                Author = updateBlogPostRequestDTO.Author,
                IsVisible = updateBlogPostRequestDTO.IsVisible
            };

            var blgPst = await _blogPostRepository.UpdateAsync(blogPost);
            if (blgPst == null)
            {
                return NotFound($"Blog Post not exist with this id {blogPost.Id}");
            }

            var response = new BlogPostDTO
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPosts(Guid id)
        {
            var blogPost = await _blogPostRepository.DeleteAsync(id);
            if (blogPost == null)
            {
                return NotFound("Category Not Found");
            }

            var response = new BlogPostDTO
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogPosts(Guid id)
        {
            var blogPost = await _blogPostRepository.GetByIdAsync(id);
            if (blogPost == null)
            {
                return NotFound($"blogPost with this id: {id} not Found!!");
            }
            var response = new BlogPostDTO
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogPosts()
        {
            var blogPosts = await _blogPostRepository.GetAsync();
            if (blogPosts == null)
            {
                return NotFound("Blog Posts Not Found");
            }
            return Ok(blogPosts);
        }
    }
}
