using back.Helper;
using back.Models.Domain;

namespace back.Repositories.Interface
{
    public interface IUploadRepository
    {
        Task<string> UploadAsync(IFormFile formFile, string fileName, string folder);
        Task<bool> RemoveAsync(string fileName, string folder, string url);
    }
}
