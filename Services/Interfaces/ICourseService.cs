using ComputerSeekho.API.Entities;
using ComputerSeekho.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSeekho.Application.Services.Interfaces
{
    public interface ICourseService
    {
        Task<CourseResponseDTO> CreateCourseWithImageAsync(CourseCreateRequestDTO dto);
        Task<CourseResponseDTO> UpdateCourseWithImageAsync(int courseId, CourseUpdateRequestDTO dto);
        Task<IEnumerable<CourseResponseDTO>> GetAllCoursesAsync();
        Task<IEnumerable<CourseResponseDTO>> GetActiveCoursesAsync();
        Task<CourseResponseDTO> GetCourseByIdAsync(int courseId);
        Task<Course> GetCourseEntityByIdAsync(int courseId);
        Task<bool> DeleteCourseAsync(int courseId);

        Task<string?> GetCourseNameByIdAsync(int courseId);

    }
}
