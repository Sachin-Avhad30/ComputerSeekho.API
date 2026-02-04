using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Repositories;
using ComputerSeekho.Application.Services.Interfaces;

namespace ComputerSeekho.Application.Services
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;

        public StaffService(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        /// <summary>
        /// Get all teaching staff (faculty)
        /// </summary>
        public async Task<List<Staff>> GetFacultyListAsync()
        {
            return await _staffRepository.GetByRoleAsync("teaching");
        }

        /// <summary>
        /// Get all staff
        /// </summary>
        public async Task<List<Staff>> GetAllStaffAsync()
        {
            return await _staffRepository.GetAllAsync();
        }

        /// <summary>
        /// Get staff by ID
        /// </summary>
        public async Task<Staff> GetStaffByIdAsync(int staffId)
        {
            var staff = await _staffRepository.GetByIdAsync(staffId);

            if (staff == null)
            {
                throw new Exception($"Staff not found with id: {staffId}");
            }

            return staff;
        }
    }
}