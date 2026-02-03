using ComputerSeekho.API.Entities;


namespace ComputerSeekho.API.Repositories.Interfaces
{
    public interface IRecruiterRepository
    {
        Task<List<RecruiterMaster>> GetAllAsync();
        Task<RecruiterMaster> GetByIdAsync(int id);
        Task AddAsync(RecruiterMaster recruiter);
        Task UpdateAsync(RecruiterMaster recruiter);
        Task DeleteAsync(RecruiterMaster recruiter);
        Task<List<RecruiterMaster>> GetActiveAsync();


    }
}
