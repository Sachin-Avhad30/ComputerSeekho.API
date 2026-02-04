using ComputerSeekho.API.Data;
using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComputerSeekho.API.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly AppDbContext _context;

        public AlbumRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Album?> GetAlbumByNameAsync(string albumName)
        {
            return await _context.Albums
                .FirstOrDefaultAsync(a => a.AlbumName == albumName);
        }

        public async Task<List<Image>> GetImagesByAlbumNameAsync(string albumName)
        {
            return await _context.Images
                .Include(i => i.Album)
                .Where(i =>
                    i.Album.AlbumName == albumName &&
                    i.ImageIsActive == true   // ✅ IMPORTANT CONDITION
                )
                .ToListAsync();
        }
        public async Task AddAlbumAsync(Album album)
        {
            await _context.Albums.AddAsync(album);
        }

        public async Task AddImagesAsync(List<Image> images)
        {
            await _context.Images.AddRangeAsync(images);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
