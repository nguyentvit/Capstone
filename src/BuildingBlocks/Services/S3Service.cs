using BuildingBlocks.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Services
{
    public class S3Service : IS3Service
    {
        private readonly string _basePath = @"E:\Capstone\Capstone.Web\capstone\src\assets\images";
        public Task DeleteFileAsync(string fileKey)
        {
            if (string.IsNullOrWhiteSpace(fileKey))
                throw new ArgumentException("FileKey không hợp lệ");

            var filePath = Path.Combine(_basePath, fileKey);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                throw new FileNotFoundException($"Không tìm thấy file: {fileKey}");
            }

            return Task.CompletedTask;
        }

        public async Task<string> UploadFileAsync(IFormFile File)
        {

            if (File == null || File.Length == 0)
                throw new ArgumentException("File không hợp lệ");

            var fileExtension = Path.GetExtension(File.FileName);
            var newFileName = $"{Guid.NewGuid()}{fileExtension}";

            var filePath = Path.Combine(_basePath, newFileName);

            Directory.CreateDirectory(_basePath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await File.CopyToAsync(stream);
            }

            return newFileName;
        }
    }
}
