using ComputerSeekho.API.Enum;
using Microsoft.AspNetCore.Http;

namespace ComputerSeekho.Application.Services.Interfaces
{
    public interface IFileStorageService
    {
        //Task<string> StoreCourseImageAsync(IFormFile file, string courseName);
        //Task<bool> DeleteFileAsync(string filePath);

        Task<string> StoreImageAsync(
            IFormFile file,
            UploadType uploadType,
            string entityName);

        Task<bool> DeleteFileAsync(string filePath);
        string GetFileUrl(string fileName);
    }
}
