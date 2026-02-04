using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Repositories.Interfaces;
using ComputerSeekho.API.Services.Interfaces;
using OfficeOpenXml;
using System.ComponentModel;

namespace ComputerSeekho.API.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _repo;

        public AlbumService(IAlbumRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<AlbumImageDto>> GetAllImagesAsync(string albumName)
        {
            var images = await _repo.GetImagesByAlbumNameAsync(albumName);

            return images.Select(i => new AlbumImageDto
            {
                AlbumName = i.Album.AlbumName,
                AlbumDescription = i.Album.AlbumDescription,
                ImagePath = i.ImagePath,
                IsAlbumCover = i.IsAlbumCover
            }).ToList();
        }

        public async Task UploadExcelAsync(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];

            int rows = worksheet.Dimension.Rows;
            var images = new List<Image>();

            for (int row = 2; row <= rows; row++)
            {
                string albumName = worksheet.Cells[row, 1].Text.Trim();

                if (string.IsNullOrEmpty(albumName))
                    continue;

                var album = await _repo.GetAlbumByNameAsync(albumName);

                if (album == null)
                {
                    album = new Album
                    {
                        AlbumName = albumName,
                        AlbumDescription = worksheet.Cells[row, 2].Text,
                        AlbumIsActive = worksheet.Cells[row, 3].Text == "1",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _repo.AddAlbumAsync(album);
                    await _repo.SaveAsync(); // AlbumId generate hoga
                }

                images.Add(new Image
                {
                    AlbumId = album.AlbumId,
                    ImagePath = worksheet.Cells[row, 4].Text,
                    IsAlbumCover = worksheet.Cells[row, 5].Text == "1",
                    ImageIsActive = worksheet.Cells[row, 6].Text == "1",
                    CreatedAt = DateTime.UtcNow
                });
            }

            if (images.Any())
            {
                await _repo.AddImagesAsync(images);
                await _repo.SaveAsync();
            }
        }
    }
}

