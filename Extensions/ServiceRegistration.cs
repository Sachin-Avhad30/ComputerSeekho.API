using ComputerSeekho.API.Data;
using ComputerSeekho.API.Repositories.Interfaces;
using ComputerSeekho.API.Repositories;
using ComputerSeekho.API.Services;
using ComputerSeekho.Application.Services.Interfaces;
using ComputerSeekho.Application.Services;
using Microsoft.EntityFrameworkCore;
using ComputerSeekho.API.Services.Interfaces;

namespace ComputerSeekho.API.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            //  DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySQL(
                    configuration.GetConnectionString("DefaultConnection")
                ));

            //  Repositories
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IBatchRepository,BatchRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IPlacementRepository, PlacementRepository>();
            services.AddScoped<IRecruiterRepository, RecruiterRepository>();

            // Services
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IBatchService, BatchService>();
            services.AddScoped<IRecruiterService, RecruiterService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IPlacementService, PlacementService>();

            return services;
        }
    }
}
