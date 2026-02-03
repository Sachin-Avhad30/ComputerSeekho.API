
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ComputerSeekho.API.Enum;
namespace ComputerSeekho.API
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var account = new Account(
                configuration["CloudinarySettings:CloudName"],
                configuration["CloudinarySettings:ApiKey"],
                configuration["CloudinarySettings:ApiSecret"]
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(
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

            var sanitizedName = SanitizeFileName(entityName);

            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = uploadType.ToString().ToLower(), // 🔥 KEY PART
                PublicId = $"{sanitizedName}_{Guid.NewGuid()}",
                Transformation = new Transformation()
                    .Quality("auto")
                    .FetchFormat("auto")
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
                throw new Exception(result.Error.Message);

            return result.SecureUrl.ToString();
        }


        private string SanitizeFileName(string name)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = string.Join("_", name.Split(invalidChars));
            return sanitized.Length > 50 ? sanitized.Substring(0, 50) : sanitized;
        }
    }

}
