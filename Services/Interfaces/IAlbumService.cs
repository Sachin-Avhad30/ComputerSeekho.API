using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using ComputerSeekho.API.DTOs;

namespace ComputerSeekho.API.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<List<AlbumImageDto>> GetAllImagesAsync(string albumName);
        Task UploadExcelAsync(IFormFile file);
    }
}
