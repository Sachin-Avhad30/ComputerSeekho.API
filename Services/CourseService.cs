using ComputerSeekho.API.Entities;
using ComputerSeekho.API.Enum;
using ComputerSeekho.API.Repositories.Interfaces;
using ComputerSeekho.Application.DTOs;
using ComputerSeekho.Application.Services.Interfaces;
namespace ComputerSeekho.API.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IFileStorageService _fileStorageService;

        public CourseService(
            ICourseRepository courseRepository,
            IFileStorageService fileStorageService)
        {
            _courseRepository = courseRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<CourseResponseDTO> CreateCourseWithImageAsync(CourseCreateRequestDTO dto)
        {
            var course = new Course
            {
                CourseName = dto.CourseName,
                CourseDescription = dto.CourseDescription,
                CourseDuration = dto.CourseDuration,
                CourseFees = dto.CourseFees ?? 0,
                CourseFeesFrom = dto.CourseFeesFrom,
                CourseFeesTo = dto.CourseFeesTo,
                CourseSyllabus = dto.CourseSyllabus,
                AgeGrpType = dto.AgeGrpType,
                VideoId = dto.VideoId,
                CourseIsActive = true
            };

            if (dto.CoverPhoto != null && dto.CoverPhoto.Length > 0)
            {
                var coverPhotoUrl = await _fileStorageService.StoreImageAsync(
                    dto.CoverPhoto,UploadType.Course ,dto.CourseName);
                course.CoverPhoto = coverPhotoUrl;
            }

            var savedCourse = await _courseRepository.AddAsync(course);
            return MapToDTO(savedCourse);
        }

        public async Task<CourseResponseDTO> UpdateCourseWithImageAsync(
            int courseId, CourseUpdateRequestDTO dto)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
                throw new KeyNotFoundException($"Course with ID {courseId} not found");

            var oldImagePath = course.CoverPhoto;

            course.CourseName = dto.CourseName;
            course.CourseDescription = dto.CourseDescription;
            course.CourseDuration = dto.CourseDuration;
            course.CourseFees = dto.CourseFees ?? course.CourseFees;
            course.CourseFeesFrom = dto.CourseFeesFrom;
            course.CourseFeesTo = dto.CourseFeesTo;
            course.CourseSyllabus = dto.CourseSyllabus;
            course.AgeGrpType = dto.AgeGrpType;
            course.VideoId = dto.VideoId;

            if (dto.CoverPhoto != null && dto.CoverPhoto.Length > 0)
            {
                if (!string.IsNullOrEmpty(oldImagePath))
                    await _fileStorageService.DeleteFileAsync(oldImagePath);

                var newImagePath = await _fileStorageService.StoreImageAsync(
                    dto.CoverPhoto,UploadType.Course,dto.CourseName);
                course.CoverPhoto = newImagePath;
            }

            var updatedCourse = await _courseRepository.UpdateAsync(course);
            return MapToDTO(updatedCourse);
        }

        public async Task<IEnumerable<CourseResponseDTO>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            return courses.Select(MapToDTO);
        }

        public async Task<IEnumerable<CourseResponseDTO>> GetActiveCoursesAsync()
        {
            var courses = await _courseRepository.GetActiveCoursesAsync();
            return courses.Select(MapToDTO);
        }

        public async Task<CourseResponseDTO> GetCourseByIdAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
                throw new KeyNotFoundException($"Course with ID {courseId} not found");
            return MapToDTO(course);
        }

        public async Task<Course> GetCourseEntityByIdAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
                throw new KeyNotFoundException($"Course with ID {courseId} not found");
            return course;
        }

        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
                throw new KeyNotFoundException($"Course with ID {courseId} not found");

            if (!string.IsNullOrEmpty(course.CoverPhoto))
                await _fileStorageService.DeleteFileAsync(course.CoverPhoto);

            return await _courseRepository.DeleteAsync(courseId);
        }

        private CourseResponseDTO MapToDTO(Course course)
        {
            return new CourseResponseDTO
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                CourseDescription = course.CourseDescription,
                CourseDuration = course.CourseDuration,
                CourseFees = course.CourseFees,
                CourseFeesFrom = course.CourseFeesFrom,
                CourseFeesTo = course.CourseFeesTo,
                CourseSyllabus = course.CourseSyllabus,
                AgeGrpType = course.AgeGrpType,
                CourseIsActive = course.CourseIsActive,
                CoverPhoto = course.CoverPhoto,
                VideoId = course.VideoId,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt
            };
        }
    }
}
