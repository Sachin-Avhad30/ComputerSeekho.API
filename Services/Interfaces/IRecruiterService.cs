using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Entities;

namespace ComputerSeekho.Application.Services.Interfaces
{
    public interface IRecruiterService
    {
        Task<List<Recruiter>> GetAllAsync();
        Task CreateAsync(RecruiterDto dto);
        Task UpdateAsync(int id, RecruiterDto dto);
        Task DeleteAsync(int id);
        Task<List<RecruiterLogoDto>> GetActiveLogosAsync();

    }
}
