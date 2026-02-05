using System.Collections.Generic;
using System.Threading.Tasks;
using ComputerSeekho.API.Entities;

namespace ComputerSeekho.API.Repositories.Interfaces
{
    public interface IAlbumRepository
    {
        Task<Album?> GetAlbumByNameAsync(string albumName);
        Task<List<Image>> GetImagesByAlbumNameAsync(string albumName);
        Task AddAlbumAsync(Album album);
        Task AddImagesAsync(List<Image> images);
        Task SaveAsync();
    }
}
