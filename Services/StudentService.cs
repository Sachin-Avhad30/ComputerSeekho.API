using ComputerSeekho.API.DTOs;
using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Enum;
using ComputerSeekho.API.Repositories.Interfaces;
using ComputerSeekho.API.Services.Interfaces;
using ComputerSeekho.Application.Services.Interfaces;

namespace ComputerSeekho.API.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        private readonly IFileStorageService _fileStorage;

        public StudentService(IStudentRepository repository, IFileStorageService fileStorage)
        {
            _repository = repository;
            _fileStorage = fileStorage;
        }

        // GET
        public async Task<List<Student>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // POST
        public async Task CreateAsync(StudentDto dto)
        {
            string? photoUrl = null;

            // 🔥 IMAGE SAVE
            if (dto.Photo != null)
            {
                photoUrl = await _fileStorage.StoreImageAsync(
                    dto.Photo,
                    UploadType.Student,
                    dto.StudentName
                );
            }

            var student = new Student
            {
                BatchId = dto.BatchId,
                CourseId = dto.CourseId,
                StudentName = dto.StudentName,
                StudentMobile = dto.StudentMobile,
                StudentGender = dto.StudentGender,
                StudentDob = dto.StudentDob,
                StudentAddress = dto.StudentAddress,
                StudentQualification = dto.StudentQualification,
                StudentUsername = dto.StudentUsername, // This IS the email
                StudentPassword = dto.StudentPassword,
                PhotoUrl = photoUrl,
                RegistrationStatus = dto.RegistrationStatus,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(student);
        }

        // PUT
        public async Task UpdateAsync(int id, StudentDto dto)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
                throw new Exception("Student not found");

            // 🔥 NEW IMAGE UPLOAD
            if (dto.Photo != null)
            {
                // ❌ OLD IMAGE DELETE
                if (!string.IsNullOrEmpty(student.PhotoUrl))
                {
                    await _fileStorage.DeleteFileAsync(student.PhotoUrl);
                }

                // ✅ NEW IMAGE SAVE
                student.PhotoUrl = await _fileStorage.StoreImageAsync(
                    dto.Photo,
                    UploadType.Student,
                    dto.StudentName
                );
            }

            // UPDATE DATA
            student.BatchId = dto.BatchId;
            student.CourseId = dto.CourseId;
            student.StudentName = dto.StudentName;
            student.StudentMobile = dto.StudentMobile;
            student.StudentGender = dto.StudentGender;
            student.StudentDob = dto.StudentDob;
            student.StudentAddress = dto.StudentAddress;
            student.StudentQualification = dto.StudentQualification;
            student.StudentUsername = dto.StudentUsername; // This IS the email
            student.StudentPassword = dto.StudentPassword;
            student.RegistrationStatus = dto.RegistrationStatus;
            student.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(student);
        }

        // DELETE
        public async Task DeleteAsync(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
                throw new Exception("Student not found");

            // 🔥 IMAGE DELETE
            if (!string.IsNullOrEmpty(student.PhotoUrl))
            {
                await _fileStorage.DeleteFileAsync(student.PhotoUrl);
            }

            await _repository.DeleteAsync(student);
        }
    }
}