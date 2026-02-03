using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Repositories.Interfaces;
using ComputerSeekho.API.Data;
using Microsoft.EntityFrameworkCore;


namespace ComputerSeekho.API.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudentMaster>> GetAllAsync()
        {
            return await _context.StudentMasters.ToListAsync();
        }

        public async Task<StudentMaster> GetByIdAsync(int id)
        {
            return await _context.StudentMasters.FindAsync(id);
        }

        public async Task AddAsync(StudentMaster student)
        {
            _context.StudentMasters.Add(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StudentMaster student)
        {
            _context.StudentMasters.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(StudentMaster student)
        {
            _context.StudentMasters.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}
