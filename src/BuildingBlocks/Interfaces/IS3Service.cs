using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Interfaces
{
    public interface IS3Service
    {
        Task<string> UploadFileAsync(IFormFile File);
        Task DeleteFileAsync(string fileKey);
    }
}
