using ComputerSeekho.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using ComputerSeekho.API.Enum;

namespace ComputerSeekho.Application.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _uploadPath;
        private readonly IWebHostEnvironment _environment;

        public FileStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "courses");

            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);
        }

        //public async Task<string> StoreCourseImageAsync(IFormFile file, string courseName)
        //{
        //    if (file == null || file.Length == 0)
        //        return null;

        //    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        //    var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        //    if (!allowedExtensions.Contains(fileExtension))
        //        throw new InvalidOperationException("Invalid file type");

        //    if (file.Length > 5 * 1024 * 1024)
        //        throw new InvalidOperationException("File size exceeds 5MB limit");

        //    var sanitizedCourseName = SanitizeFileName(courseName);
        //    var uniqueFileName = $"{sanitizedCourseName}_{Guid.NewGuid()}{fileExtension}";
        //    var filePath = Path.Combine(_uploadPath, uniqueFileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    return $"/uploads/courses/{uniqueFileName}";
        //}

        //public async Task<bool> DeleteFileAsync(string filePath)
        //{
        //    if (string.IsNullOrEmpty(filePath))
        //        return false;

        //    try
        //    {
        //        var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
        //        if (File.Exists(fullPath))
        //        {
        //            await Task.Run(() => File.Delete(fullPath));
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}


        public async Task<string> StoreImageAsync(
            IFormFile file,
            UploadType uploadType,
            string entityName)
        {
            if (file == null || file.Length == 0)
                return null;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
                throw new InvalidOperationException("Invalid file type");

            if (file.Length > 5 * 1024 * 1024)
                throw new InvalidOperationException("File size exceeds 5MB limit");

            // 🔥 SAFETY: WebRootPath fallback
            var webRoot = _environment.WebRootPath
                          ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            var folderName = uploadType.ToString().ToLower(); // course, student, faculty...
            var uploadPath = Path.Combine(webRoot, "uploads", folderName);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var sanitizedName = SanitizeFileName(entityName);
            var uniqueFileName = $"{sanitizedName}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{folderName}/{uniqueFileName}";
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            var webRoot = _environment.WebRootPath
                          ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            var fullPath = Path.Combine(webRoot, filePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
                return true;
            }

            return false;
        }

        private string SanitizeFileName(string name)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = string.Join("_", name.Split(invalidChars));
            return sanitized.Length > 50 ? sanitized.Substring(0, 50) : sanitized;
        }
        public string GetFileUrl(string fileName)
        {
            return $"/uploads/courses/{fileName}";
        }

        //private string SanitizeFileName(string fileName)
        //{
        //    var invalidChars = Path.GetInvalidFileNameChars();
        //    var sanitized = string.Join("_", fileName.Split(invalidChars));
        //    return sanitized.Length > 50 ? sanitized.Substring(0, 50) : sanitized;
        //}

    }
}
