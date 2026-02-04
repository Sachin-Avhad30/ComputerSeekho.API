using ComputerSeekho.API.Entities;


namespace ComputerSeekho.API.Repositories.Interfaces
{
    public interface IRecruiterRepository
    {
        Task<List<Recruiter>> GetAllAsync();
        Task<Recruiter> GetByIdAsync(int id);
        Task AddAsync(Recruiter recruiter);
        Task UpdateAsync(Recruiter recruiter);
        Task DeleteAsync(Recruiter recruiter);
        Task<List<Recruiter>> GetActiveAsync();


    }
}
