using back.Helper;
using back.Models.Domain;
using back.Repositories.Interface;
using Microsoft.AspNetCore.Hosting;

namespace back.Repositories.Implementation
{
    public class UploadRepository : IUploadRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly BaseUrlService _baseUrlService;

        public UploadRepository(IWebHostEnvironment webHostEnvironment, BaseUrlService baseUrlService)
        {
            _webHostEnvironment = webHostEnvironment;
            _baseUrlService = baseUrlService;
        }

        public Task<bool> RemoveAsync(string fileName, string folder, string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", folder, fileName + "." + url.Split(".")[1]);
                try
                {
                    if (File.Exists(FilePath))
                    {
                        File.Delete(FilePath);
                        return Task.FromResult(true);
                    }
                }
                catch { return Task.FromResult(false); }
            }
            return Task.FromResult(false);
        }

        public async Task<string> UploadAsync(HttpRequest request, string folder)
        {
            bool Results = false;
            string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", folder);
            var file = request.Form.Files[0];
            try
            {
                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }

                string[] filesWithSameName = Directory.GetFiles(FilePath, file.Name + ".*");
                foreach (string fileWithSameName in filesWithSameName)
                {
                    if (File.Exists(fileWithSameName))
                    {
                        File.Delete(fileWithSameName);
                    }
                }

                string FileExtension = Path.GetExtension(file.FileName);
                string ImagePath = Path.Combine(FilePath, file.Name + FileExtension);

                using FileStream fileStream = File.Create(ImagePath);
                await file.CopyToAsync(fileStream);
                Results = true;
            }
            catch
            {
                return Path.Combine(_baseUrlService.BaseUrl, "Uploads", "Common", "no_image.png");
            }

            return Results
                ? Path.Combine(_baseUrlService.BaseUrl, "Uploads", folder, file.Name + Path.GetExtension(file.FileName))
                : Path.Combine(_baseUrlService.BaseUrl, "Uploads", "Common", "no_image.png");
        }
        public async Task<string> UploadAsync(IFormFile formFile, string fileName, string folder)
        {
            bool Results = false;
            string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", folder);
            try
            {
                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }

                string[] filesWithSameName = Directory.GetFiles(FilePath, fileName + ".*");
                foreach (string fileWithSameName in filesWithSameName)
                {
                    if (File.Exists(fileWithSameName))
                    {
                        File.Delete(fileWithSameName);
                    }
                }

                string FileExtension = Path.GetExtension(formFile.FileName);
                string ImagePath = Path.Combine(FilePath, fileName + FileExtension);

                using FileStream fileStream = File.Create(ImagePath);
                await formFile.CopyToAsync(fileStream);
                Results = true;
            }
            catch
            {
                return Path.Combine("Uploads", "Common", "no_image.png");
            }

            return Results
                ? Path.Combine("Uploads", folder, fileName + Path.GetExtension(formFile.FileName))
                : Path.Combine("Uploads", "Common", "no_image.png");
        }
    }
}
